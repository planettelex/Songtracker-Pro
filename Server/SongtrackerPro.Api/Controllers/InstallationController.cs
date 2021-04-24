using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SongtrackerPro.Api.Attributes;
using SongtrackerPro.Api.ViewModels;
using SongtrackerPro.Data.Enums;
using SongtrackerPro.Tasks.InstallationTasks;
using SongtrackerPro.Tasks.UserTasks;
using SongtrackerPro.Utilities;

namespace SongtrackerPro.Api.Controllers
{
    [ApiController]
    public class InstallationController : ApiControllerBase
    {
        public InstallationController(IGetUserByAuthenticationTokenTask getUserByAuthenticationTokenTask,
                                      IGetInstallationTask getInstallationTask,
                                      ISeedSystemDataTask seedSystemDataTask) : 
        base(getUserByAuthenticationTokenTask)
        {
            _seedSystemDataTask = seedSystemDataTask;
            _getInstallationTask = getInstallationTask;
        }
        private readonly ISeedSystemDataTask _seedSystemDataTask;
        private readonly IGetInstallationTask _getInstallationTask;

        [Route(Routes.Root)]
        [HttpGet]
        public IActionResult GetApiInfo()
        {
            var apiInfo = new ApiInfo
            {
                Version = ApplicationSettings.Version,
                Documentation = ApplicationSettings.Api.Documentation
            };
            var taskResults = _getInstallationTask.DoTask(null);
            apiInfo.Name = taskResults.Success ? taskResults.Data.Name : SystemMessage("INSTALLATION_NOT_FOUND");

            return Ok(JsonSerialize(apiInfo));
        }

        [Route(Routes.System)]
        [HttpGet]
        [UserTypesAllowed(UserType.SystemAdministrator)]
        public IActionResult GetInstallationInfo()
        {
            if (!UserIsAuthorized(MethodBase.GetCurrentMethod()))
                return Unauthorized();

            var taskResults = _getInstallationTask.DoTask(null);

            if (taskResults.Success)
                return Ok(JsonSerialize(taskResults));

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [Route(Routes.SystemSeed)]
        [HttpPost]
        [UserTypesAllowed(UserType.SystemAdministrator)]
        public IActionResult SeedSystemData()
        {
            if (!UserIsAuthorized(MethodBase.GetCurrentMethod()))
                return Unauthorized();

            var taskResults = _seedSystemDataTask.DoTask(null);

            if (taskResults.Success)
                return Ok(JsonSerialize(taskResults));

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
