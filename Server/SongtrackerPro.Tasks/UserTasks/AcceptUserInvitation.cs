using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Enums;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Resources;
using SongtrackerPro.Tasks.ArtistTasks;
using SongtrackerPro.Tasks.InstallationTasks;
using SongtrackerPro.Tasks.PersonTasks;
using SongtrackerPro.Utilities;
using SongtrackerPro.Utilities.Services;

namespace SongtrackerPro.Tasks.UserTasks
{
    public interface IAcceptUserInvitationTask : ITask<UserInvitation, User> { }

    public class AcceptUserInvitation : TaskBase, IAcceptUserInvitationTask
    {
        public AcceptUserInvitation(ApplicationDbContext dbContext,
                                    IEmailService emailService,
                                    IHtmlService htmlService,
                                    ITokenService tokenService,
                                    IAddPersonTask addPersonTask,
                                    IAddArtistMemberTask addArtistMember,
                                    IAddArtistManagerTask addArtistManager,
                                    IGetInstallationTask getInstallationTask)
        {
            _dbContext = dbContext;
            _emailService = emailService;
            _htmlService = htmlService;
            _tokenService = tokenService;
            _addPersonTask = addPersonTask;
            _addArtistMember = addArtistMember;
            _addArtistManager = addArtistManager;
            _getInstallationTask = getInstallationTask;
        }
        private readonly ApplicationDbContext _dbContext;
        private readonly IEmailService _emailService;
        private readonly IHtmlService _htmlService;
        private readonly ITokenService _tokenService;
        private readonly IAddPersonTask _addPersonTask;
        private readonly IAddArtistMemberTask _addArtistMember;
        private readonly IAddArtistManagerTask _addArtistManager;
        private readonly IGetInstallationTask _getInstallationTask;
        
        public TaskResult<User> DoTask(UserInvitation userInvitation)
        {
            try
            {
                var invitation = _dbContext.UserInvitations.Where(ui => ui.Uuid == userInvitation.Uuid)
                    .Include(ui => ui.InvitedByUser)
                    .Single();

                if (invitation.Type == UserType.Unassigned)
                    throw new TaskException(SystemMessage("USER_INVITATION_UNASSIGNED"));

                var newUser = userInvitation.CreatedUser;
                newUser.AuthenticationId = invitation.Email;
                newUser.Type = invitation.Type;
                newUser.Roles = invitation.Roles;

                var newPerson = newUser.Person;
                newPerson.Email = invitation.Email;
                var addPersonResult = _addPersonTask.DoTask(newPerson);
                if (!addPersonResult.Success)
                    throw addPersonResult.Exception;

                var personId = addPersonResult.Data;
                var proId = newUser.PerformingRightsOrganization?.Id ?? newUser.PerformingRightsOrganizationId;
                var publisherId = invitation.Publisher?.Id ?? invitation.PublisherId;
                var recordLabelId = invitation.RecordLabel?.Id ?? invitation.RecordLabelId;
                var artistId = invitation.Artist?.Id ?? invitation.ArtistId;

                newUser.Person = null;
                newUser.PersonId = personId;
                newUser.PerformingRightsOrganization = null;
                newUser.PerformingRightsOrganizationId = proId;
                newUser.Publisher = null;
                newUser.PublisherId = publisherId;
                newUser.RecordLabel = null;
                newUser.RecordLabelId = recordLabelId;

                _dbContext.Users.Add(newUser);
                _dbContext.SaveChanges();

                invitation.CreatedUser = null;
                invitation.CreatedUserId = newUser.Id;
                invitation.AcceptedOn = DateTime.UtcNow;
                _dbContext.SaveChanges();

                invitation.CreatedUser = newUser;

                if (artistId.HasValue)
                {
                    var artist = _dbContext.Artists.Single(a => a.Id == artistId.Value);
                    if (invitation.Roles.HasFlag(SystemUserRoles.ArtistMember))
                    {
                        var artistMember = new ArtistMember
                        {
                            Artist = artist,
                            Member = newPerson,
                            StartedOn = DateTime.Today
                        };
                        var addArtistMemberResult = _addArtistMember.DoTask(artistMember);
                        if (!addArtistMemberResult.Success)
                            throw addArtistMemberResult.Exception;
                    }
                    else if (invitation.Roles.HasFlag(SystemUserRoles.ArtistManager))
                    {
                        var artistManager = new ArtistManager
                        {
                            Artist = artist,
                            Manager = newPerson,
                            StartedOn = DateTime.Today
                        };
                        var addArtistManagerResult = _addArtistManager.DoTask(artistManager);
                        if (!addArtistManagerResult.Success)
                            throw addArtistManagerResult.Exception;
                    }
                }

                newUser.Person = newPerson;
                newUser.PerformingRightsOrganization = proId > 0 ?
                    _dbContext.PerformingRightsOrganizations.Where(p => p.Id == proId)
                        .Include(p => p.Country)
                        .SingleOrDefault() : null;
                newUser.Publisher = publisherId > 0 ?
                    _dbContext.Publishers.Where(p => p.Id == publisherId)
                        .Include(p => p.Address).ThenInclude(a => a.Country)
                        .SingleOrDefault() : null;
                newUser.RecordLabel = recordLabelId > 0 ? 
                    _dbContext.RecordLabels.Where(l => l.Id == recordLabelId)
                        .Include(p => p.Address).ThenInclude(a => a.Country)
                        .SingleOrDefault() : null;

                userInvitation.LoginLink = ApplicationSettings.Web.Root + WebRoutes.Login;
                var installation = _getInstallationTask.DoTask(null).Data;
                var emailTemplate = EmailTemplate($"{invitation.Type}Welcome.html");
                var body = ReplaceTokens(emailTemplate, invitation, installation);
                var subject = ReplaceTokens(_htmlService.GetTitle(emailTemplate), invitation, installation);

                _emailService.SendEmail(newUser.Person.FirstAndLastName, newUser.Person.Email, 
                    installation.Name, ApplicationSettings.Mail.From, subject, body);

                return new TaskResult<User>(newUser);
            }
            catch (Exception e)
            {
                return new TaskResult<User>(new TaskException(e));
            }
        }

        private string ReplaceTokens(string template, UserInvitation userInvitation, Installation installation)
        {
            var replaced = _tokenService.ReplaceTokens(template, installation);
            replaced = _tokenService.ReplaceTokens(replaced, userInvitation);
            replaced = _tokenService.ReplaceTokens(replaced, userInvitation.CreatedUser);
            replaced = _tokenService.ReplaceTokens(replaced, userInvitation.CreatedUser.Person);

            switch (userInvitation.Type)
            {
                case UserType.SystemUser:
                    replaced = _tokenService.ReplaceTokens(replaced, userInvitation.Artist);
                    break;
                case UserType.PublisherAdministrator:
                    replaced = _tokenService.ReplaceTokens(replaced, userInvitation.Publisher);
                    break;
                case UserType.LabelAdministrator:
                    replaced = _tokenService.ReplaceTokens(replaced, userInvitation.RecordLabel);
                    break;
            }

            return replaced;
        }
    }
}
