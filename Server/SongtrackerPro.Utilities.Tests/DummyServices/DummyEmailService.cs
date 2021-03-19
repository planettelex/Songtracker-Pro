using System.Collections.Generic;
using SongtrackerPro.Utilities.Services;

namespace SongtrackerPro.Utilities.Tests.DummyServices
{
    public class DummyEmailService : IEmailService
    {
        public void SendEmail(string to, string title, string body, List<string> attachmentPaths = null) { }
    }
}
