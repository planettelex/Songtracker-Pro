using SongtrackerPro.Resources;
using SongtrackerPro.Utilities;

namespace SongtrackerPro.Tasks
{
    public abstract class TaskBase
    {
        protected string SystemMessage(string key)
        {
            return GetResource.SystemMessage(ApplicationSettings.Api.Culture, key);
        }

        protected string SeedData(string key)
        {
            return GetResource.SeedData(ApplicationSettings.Api.Culture, key);
        }

        protected string EmailTemplate(string filename)
        {
            return GetResource.EmailTemplate(ApplicationSettings.Api.Culture, filename);
        }
    }
}
