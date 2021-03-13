using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using SongtrackerPro.Tasks.InstallationTasks;

namespace SongtrackerPro.Api.Controllers
{
    [ApiController]
    public class InstallationController : ApiController
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
            var taskResults = _getInstallationTask.DoTask(null);

            return taskResults.Success ?
                JsonSerializer.Serialize(taskResults.Data, SerializerOptions) : 
                JsonSerializer.Serialize(taskResults.Exception, SerializerOptions);
        }
    }
}
