using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SongtrackerPro.Api.Attributes;
using SongtrackerPro.Data.Enums;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Tasks.PlatformTasks;
using SongtrackerPro.Tasks.UserTasks;

namespace SongtrackerPro.Api.Controllers
{
    [ApiController]
    public class PlatformController : ApiControllerBase
    {
        public PlatformController(IGetLoginTask getLoginTask,
                                  IListServicesTask listServicesTask, 
                                  IListPlatformsTask listPlatformsTask,
                                  IGetPlatformTask getPlatformTask,
                                  IAddPlatformTask addPlatformTask,
                                  IUpdatePlatformTask updatePlatformTask) :
        base(getLoginTask)
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
        [UserTypesAllowed(UserType.Unassigned)]
        public IActionResult ListServices()
        {
            if (!UserIsAuthorized(MethodBase.GetCurrentMethod()))
                return Unauthorized();

            var taskResults = _listServicesTask.DoTask(null);

            if (taskResults.Success)
                return Ok(JsonSerialize(taskResults));

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [Route(Routes.Platforms)]
        [HttpPost]
        [UserTypesAllowed(UserType.SystemAdministrator)]
        public IActionResult AddPlatform(Platform platform)
        {
            if (!UserIsAuthorized(MethodBase.GetCurrentMethod()))
                return Unauthorized();

            var taskResults = _addPlatformTask.DoTask(platform);

            if (taskResults.Success)
                return Ok(JsonSerialize(taskResults));

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [Route(Routes.Platforms)]
        [HttpGet]
        [UserTypesAllowed(UserType.Unassigned)]
        public IActionResult ListPlatforms()
        {
            if (!UserIsAuthorized(MethodBase.GetCurrentMethod()))
                return Unauthorized();

            var taskResults = _listPlatformsTask.DoTask(null);

            if (taskResults.Success)
                return Ok(JsonSerialize(taskResults));

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [Route(Routes.Platform)]
        [HttpGet]
        [UserTypesAllowed(UserType.Unassigned)]
        public IActionResult GetPlatform(int id)
        {
            if (!UserIsAuthorized(MethodBase.GetCurrentMethod()))
                return Unauthorized();

            var taskResults = _getPlatformTask.DoTask(id);

            if (taskResults.Success)
                return Ok(JsonSerialize(taskResults));

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [Route(Routes.Platform)]
        [HttpPut]
        [UserTypesAllowed(UserType.SystemAdministrator)]
        public IActionResult UpdatePlatform(int id, Platform platform)
        {
            if (!UserIsAuthorized(MethodBase.GetCurrentMethod()))
                return Unauthorized();

            platform.Id = id;
            var taskResults = _updatePlatformTask.DoTask(platform);

            if (taskResults.Success)
                return Ok();

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
