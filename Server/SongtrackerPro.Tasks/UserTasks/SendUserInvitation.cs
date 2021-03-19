using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Enums;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.InstallationTasks;
using SongtrackerPro.Utilities.Services;

namespace SongtrackerPro.Tasks.UserTasks
{
    public interface ISendUserInvitationTask : ITask<UserInvitation, Guid> { }

    public class SendUserInvitation : TaskBase, ISendUserInvitationTask
    {
        public SendUserInvitation(ApplicationDbContext dbContext, 
                                  IEmailService emailService,
                                  IHtmlService htmlService,
                                  ITokenService tokenService,
                                  IGetInstallationTask getInstallationTask)
        {
            _dbContext = dbContext;
            _emailService = emailService;
            _htmlService = htmlService;
            _tokenService = tokenService;
            _getInstallationTask = getInstallationTask;
        }
        private readonly ApplicationDbContext _dbContext;
        private readonly IEmailService _emailService;
        private readonly IHtmlService _htmlService;
        private readonly ITokenService _tokenService;
        private readonly IGetInstallationTask _getInstallationTask;

        public TaskResult<Guid> DoTask(UserInvitation userInvitation)
        {
            try
            {
                userInvitation.Uuid = Guid.NewGuid();
                userInvitation.SentOn = DateTime.UtcNow;

                var invitedByUserId = userInvitation.InvitedByUser?.Id ?? userInvitation.InvitedByUserId;
                var publisherId = userInvitation.Publisher?.Id ?? userInvitation.PublisherId;
                var recordLabelId = userInvitation.RecordLabel?.Id ?? userInvitation.RecordLabelId;
                var artistId = userInvitation.Artist?.Id ?? userInvitation.ArtistId;

                userInvitation.InvitedByUser = null;
                userInvitation.InvitedByUserId = invitedByUserId;
                userInvitation.Publisher = null;
                userInvitation.PublisherId = publisherId;
                userInvitation.RecordLabel = null;
                userInvitation.RecordLabelId = recordLabelId;
                userInvitation.Artist = null;
                userInvitation.ArtistId = artistId;

                _dbContext.UserInvitations.Add(userInvitation);
                _dbContext.SaveChanges();

                userInvitation.InvitedByUser = _dbContext.Users.Where(u => u.Id == invitedByUserId)
                    .Include(u => u.Person).ThenInclude(p => p.Address).ThenInclude(a => a.Country)
                    .SingleOrDefault();

                if (publisherId != null)
                    userInvitation.Publisher = _dbContext.Publishers.Where(p => p.Id == publisherId)
                        .Include(p => p.PerformingRightsOrganization)
                        .Include(p => p.Address).ThenInclude(a => a.Country)
                        .SingleOrDefault();

                if (recordLabelId != null)
                    userInvitation.RecordLabel = _dbContext.RecordLabels.Where(p => p.Id == recordLabelId)
                        .Include(l => l.Address).ThenInclude(a => a.Country)
                        .SingleOrDefault();

                if (artistId != null)
                    userInvitation.Artist = _dbContext.Artists.SingleOrDefault(p => p.Id == artistId);

                var emailTemplate = EmailTemplate($"{userInvitation.Type}Invitation.html");
                var body = ReplaceTokens(emailTemplate, userInvitation);
                var subject = _htmlService.GetTitle(emailTemplate);
                _emailService.SendEmail(userInvitation.Email, subject, body);

                return new TaskResult<Guid>(userInvitation.Uuid);
            }
            catch (Exception e)
            {
                return new TaskResult<Guid>(new TaskException(e));
            }
        }

        private string ReplaceTokens(string template, UserInvitation userInvitation)
        {
            var installation = _getInstallationTask.DoTask(null).Data;
            var replaced = _tokenService.ReplaceTokens(template, installation);
            replaced = _tokenService.ReplaceTokens(replaced, userInvitation.InvitedByUser);
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
