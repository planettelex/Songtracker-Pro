using Microsoft.AspNetCore.Mvc;
using SongtrackerPro.Tasks.InstallationTasks;

namespace SongtrackerPro.Api.Controllers
{
    [ApiController]
    public class InstallationController : ApiControllerBase
    {
        public InstallationController(IGetInstallationInfoTask getInstallationTask)
        {
            _getInstallationTask = getInstallationTask;
        }
        private readonly IGetInstallationInfoTask _getInstallationTask;

        [Route(Routes.Root)]
        [HttpGet]
        public string Get()
        {
            var taskResults = _getInstallationTask.DoTask(null);

            return JsonSerialize(taskResults);
        }
    }
}
