using System.Collections.Generic;
using System.Net;  
using System.Net.Mail;

namespace SongtrackerPro.Utilities.Services
{
    public interface IEmailService
    {
        public void SendEmail(string to, string subject, string body, List<string> attachmentPaths = null);
    }

    public class EmailService : IEmailService
    {
        public void SendEmail(string to, string subject, string body, List<string> attachmentPaths = null)
        {
            using var mail = new MailMessage { From = new MailAddress(ApplicationSettings.Mail.From) };

            mail.To.Add(to);
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = true;

            if (attachmentPaths != null)
                foreach (var attachmentPath in attachmentPaths)
                    mail.Attachments.Add(new Attachment(attachmentPath));

            using var smtp = new SmtpClient(ApplicationSettings.Mail.Smtp, ApplicationSettings.Mail.Port)
            {
                Credentials = new NetworkCredential(ApplicationSettings.Mail.From, ApplicationSettings.Mail.Password),
                EnableSsl = ApplicationSettings.Mail.EnableSsl
            };

            smtp.Send(mail);
        }
    }
}
