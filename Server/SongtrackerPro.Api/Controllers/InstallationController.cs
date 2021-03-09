using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using SongtrackerPro.Tasks.InstallationTasks;

namespace SongtrackerPro.Api.Controllers
{
    [ApiController]
    public class InstallationController : ControllerBase
    {
        public InstallationController(IGetInstallationTask getInstallationTask)
        {
            _getInstallationTask = getInstallationTask;
        }
        private readonly IGetInstallationTask _getInstallationTask;

        [Route("")]
        [HttpGet]
        public string Get()
        {
            var taskResults = _getInstallationTask.DoTask();

            return taskResults.Success ?
                JsonSerializer.Serialize(taskResults.Data) : 
                JsonSerializer.Serialize(taskResults.Exception);
        }
    }
}
