using System.Collections.Generic;
using SongtrackerPro.Utilities.Services;

namespace SongtrackerPro.Utilities.Tests.DummyServices
{
    public class DummyEmailService : IEmailService
    {
        public void SendEmail(string toName, string toEmail, string fromName, string fromEmail, 
            string subject, string body, List<string> attachmentPaths = null) { }
    }
}
