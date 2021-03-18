using Microsoft.AspNetCore.Mvc;
using SongtrackerPro.Tasks.InstallationTasks;

namespace SongtrackerPro.Api.Controllers
{
    [ApiController]
    public class InstallationController : ApiControllerBase
    {
        public InstallationController(IGetInstallationTask getInstallationTask,
                                      ISeedSystemDataTask seedSystemDataTask)
        {
            _seedSystemDataTask = seedSystemDataTask;
            _getInstallationTask = getInstallationTask;
        }
        private readonly ISeedSystemDataTask _seedSystemDataTask;
        private readonly IGetInstallationTask _getInstallationTask;

        [Route(Routes.Root)]
        [HttpPost]
        public string SeedSystemData()
        {
            var taskResults = _seedSystemDataTask.DoTask(null);

            return JsonSerialize(taskResults);
        }

        [Route(Routes.Root)]
        [HttpGet]
        public string GetInstallationInfo()
        {
            var taskResults = _getInstallationTask.DoTask(null);

            return JsonSerialize(taskResults);
        }
    }
}
