using Microsoft.AspNetCore.Mvc;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Tasks.PlatformTasks;

namespace SongtrackerPro.Api.Controllers
{
    [ApiController]
    public class PlatformController : ApiControllerBase
    {
        public PlatformController(IListServicesTask listServicesTask, 
                                  IListPlatformsTask listPlatformsTask,
                                  IGetPlatformTask getPlatformTask,
                                  IAddPlatformTask addPlatformTask,
                                  IUpdatePlatformTask updatePlatformTask)
        {
            _listServicesTask = listServicesTask;
            _listPlatformsTask = listPlatformsTask;
            _getPlatformTask = getPlatformTask;
            _addPlatformTask = addPlatformTask;
            _updatePlatformTask = updatePlatformTask;

        }
        private readonly IListServicesTask _listServicesTask;
        private readonly IListPlatformsTask _listPlatformsTask;
        private readonly IGetPlatformTask _getPlatformTask;
        private readonly IAddPlatformTask _addPlatformTask;
        private readonly IUpdatePlatformTask _updatePlatformTask;

        [Route(Routes.Services)]
        [HttpGet]
        public string ListServices()
        {
            var taskResults = _listServicesTask.DoTask(null);

            return JsonSerialize(taskResults);
        }

        [Route(Routes.Platforms)]
        [HttpPost]
        public string AddPlatform(Platform platform)
        {
            var taskResults = _addPlatformTask.DoTask(platform);

            return JsonSerialize(taskResults);
        }

        [Route(Routes.Platforms)]
        [HttpGet]
        public string ListPlatforms()
        {
            var taskResults = _listPlatformsTask.DoTask(null);

            return JsonSerialize(taskResults);
        }

        [Route(Routes.Platform)]
        [HttpGet]
        public string GetPlatform(int id)
        {
            var taskResults = _getPlatformTask.DoTask(id);

            return JsonSerialize(taskResults);
        }

        [Route(Routes.Platform)]
        [HttpPut]
        public void UpdatePlatform(int id, Platform platform)
        {
            platform.Id = id;
            _updatePlatformTask.DoTask(platform);
        }
    }
}
