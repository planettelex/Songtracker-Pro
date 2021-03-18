using SongtrackerPro.Utilities.Services;

namespace SongtrackerPro.Utilities.Tests.DummyServices
{

    public class DummyEmailService : IEmailService
    {
        public bool SendEmail(string to, string title, string body)
        {
            return true;
        }
    }
}
