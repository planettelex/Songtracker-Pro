using System;

namespace SongtrackerPro.Utilities.Services
{
    public interface IEmailService
    {
        public bool SendEmail(string to, string title, string body);
    }

    public class EmailService : IEmailService
    {
        public bool SendEmail(string to, string title, string body)
        {
            return true;
        }
    }
}
