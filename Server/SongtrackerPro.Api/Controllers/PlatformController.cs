using Microsoft.AspNetCore.Mvc;
using SongtrackerPro.Tasks.PlatformTasks;

namespace SongtrackerPro.Api.Controllers
{
    [ApiController]
    public class PlatformController : ApiControllerBase
    {
        public PlatformController(IListServicesTask listServicesTask)
        {
            _listServicesTask = listServicesTask;
        }
        private readonly IListServicesTask _listServicesTask;

        [Route(Routes.Services)]
        [HttpGet]
        public string Get()
        {
            var taskResults = _listServicesTask.DoTask(null);

            return JsonSerialize(taskResults);
        }
    }
}
