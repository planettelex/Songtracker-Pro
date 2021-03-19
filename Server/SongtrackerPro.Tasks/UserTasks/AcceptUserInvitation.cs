using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Enums;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.ArtistTasks;
using SongtrackerPro.Tasks.PersonTasks;
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
                                    IAddArtistManagerTask addArtistManager)
        {
            _dbContext = dbContext;
            _emailService = emailService;
            _htmlService = htmlService;
            _tokenService = tokenService;
            _addPersonTask = addPersonTask;
            _addArtistMember = addArtistMember;
            _addArtistManager = addArtistManager;
        }
        private readonly ApplicationDbContext _dbContext;
        private readonly IEmailService _emailService;
        private readonly IHtmlService _htmlService;
        private readonly ITokenService _tokenService;
        private readonly IAddPersonTask _addPersonTask;
        private readonly IAddArtistMemberTask _addArtistMember;
        private readonly IAddArtistManagerTask _addArtistManager;
        
        public TaskResult<User> DoTask(UserInvitation userInvitation)
        {
            try
            {
                var newUser = userInvitation.CreatedUser;
                var newPerson = newUser.Person;
                var addPersonResult = _addPersonTask.DoTask(newPerson);
                if (!addPersonResult.Success)
                    throw addPersonResult.Exception;

                var personId = addPersonResult.Data;
                var proId = newUser.PerformingRightsOrganization?.Id ?? newUser.PerformingRightsOrganizationId;
                var publisherId = userInvitation.Publisher?.Id ?? userInvitation.PublisherId;
                var recordLabelId = userInvitation.RecordLabel?.Id ?? userInvitation.RecordLabelId;
                var artistId = userInvitation.Artist?.Id ?? userInvitation.ArtistId;

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

                if (artistId.HasValue)
                {
                    var artist = _dbContext.Artists.Single(a => a.Id == artistId.Value);
                    switch (userInvitation.Type)
                    {
                        case UserType.ArtistMember:
                        {
                            var artistMember = new ArtistMember
                            {
                                Artist = artist,
                                Member = newPerson,
                                StartedOn = DateTime.Today
                            };
                            _addArtistMember.DoTask(artistMember);
                            break;
                        }
                        case UserType.ArtistManager:
                        {
                            var artistManager = new ArtistManager
                            {
                                Artist = artist,
                                Manager = newPerson,
                                StartedOn = DateTime.Today
                            };
                            _addArtistManager.DoTask(artistManager);
                            break;
                        }
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

                var emailTemplate = EmailTemplate($"{userInvitation.Type}Welcome.html");
                var body = ReplaceTokens(emailTemplate, userInvitation);
                var subject = _htmlService.GetTitle(emailTemplate);
                _emailService.SendEmail(newUser.Person.Email, subject, body);

                return new TaskResult<User>(newUser);
            }
            catch (Exception e)
            {
                return new TaskResult<User>(new TaskException(e));
            }
        }

        private string ReplaceTokens(string template, UserInvitation userInvitation)
        {
            var replaced = _tokenService.ReplaceTokens(template, userInvitation.InvitedByUser);
            replaced = _tokenService.ReplaceTokens(replaced, userInvitation.InvitedByUser.Person);

            switch (userInvitation.Type)
            {
                case UserType.ArtistMember:
                case UserType.ArtistManager:
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
