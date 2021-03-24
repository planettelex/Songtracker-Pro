using System.Collections.Generic;
using MailKit.Net.Smtp;
using MimeKit;

namespace SongtrackerPro.Utilities.Services
{
    public interface IEmailService
    {
        public void SendEmail(string toName, string toEmail, string fromName, string fromEmail, string subject, string body, List<string> attachmentPaths = null);
    }

    public class EmailService : IEmailService
    {
        public void SendEmail(string toName, string toEmail, string fromName, string fromEmail, string subject, string body, List<string> attachmentPaths = null)
        {
            var mailMessage = new MimeMessage();
            mailMessage.From.Add(new MailboxAddress(fromName, fromEmail));
            mailMessage.To.Add(new MailboxAddress(toName, toEmail));
            mailMessage.Subject = subject;

            var builder = new BodyBuilder { HtmlBody = body };
            if (attachmentPaths != null)
                foreach (var attachmentPath in attachmentPaths)
                    builder.Attachments.Add(attachmentPath);

            mailMessage.Body = builder.ToMessageBody();

            using var client = new SmtpClient();
            client.Connect(ApplicationSettings.Mail.Smtp, ApplicationSettings.Mail.Port, ApplicationSettings.Mail.EnableSsl);
            client.Authenticate(ApplicationSettings.Mail.From, ApplicationSettings.Mail.Password);
            client.Send(mailMessage);
            client.Disconnect(true);
        }
    }
}
