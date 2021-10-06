using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Enums;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Resources;
using SongtrackerPro.Tasks.InstallationTasks;
using SongtrackerPro.Utilities;
using SongtrackerPro.Utilities.Services;

namespace SongtrackerPro.Tasks.UserTasks
{
    public interface  IResendUserInvitationTask : ITask<Guid, UserInvitation> { }

    public class ResendUserInvitation : TaskBase, IResendUserInvitationTask
    {
        public ResendUserInvitation(ApplicationDbContext dbContext,
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

        public TaskResult<UserInvitation> DoTask(Guid userInvitationUuid)
        {
            try
            {
                var userInvitation = _dbContext.UserInvitations.Where(i => i.Uuid == userInvitationUuid)
                    .Include(i => i.InvitedByUser)
                    .Include(i => i.CreatedUser)
                    .SingleOrDefault();

                if (userInvitation == null)
                    return new TaskResult<UserInvitation>(false);

                if (userInvitation.AcceptedOn.HasValue || userInvitation.CreatedUser != null)
                    throw new ApplicationException(SystemMessage("USER_INVITATION_CANNOT_RESEND_ACCEPTED"));

                userInvitation.SentOn = DateTime.UtcNow;
                _dbContext.SaveChanges();

                userInvitation.AcceptLink = ApplicationSettings.Web.Domain + string.Format(WebRoutes.Join, userInvitation.Uuid);

                var emailTemplate = EmailTemplate($"{userInvitation.UserType}Invitation.html");
                var installation = _getInstallationTask.DoTask(null).Data;
                var body = ReplaceTokens(emailTemplate, userInvitation, installation);
                var subject = ReplaceTokens(_htmlService.GetTitle(emailTemplate), userInvitation, installation);

                _emailService.SendEmail(userInvitation.Name, userInvitation.Email, 
                    installation.Name, ApplicationSettings.Mail.From, subject, body);

                return new TaskResult<UserInvitation>(userInvitation);
            }
            catch (Exception e)
            {
                return new TaskResult<UserInvitation>(new TaskException(e));
            }
        }

        private string ReplaceTokens(string template, UserInvitation userInvitation, Installation installation)
        {
            var replaced = _tokenService.ReplaceTokens(template, installation);
            replaced = _tokenService.ReplaceTokens(replaced, userInvitation);
            replaced = _tokenService.ReplaceTokens(replaced, userInvitation.InvitedByUser);

            switch (userInvitation.UserType)
            {
                case UserType.SystemUser:
                    if (userInvitation.UserRoles.HasFlag(SystemUserRoles.ArtistMember) || userInvitation.UserRoles.HasFlag(SystemUserRoles.ArtistManager))
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
