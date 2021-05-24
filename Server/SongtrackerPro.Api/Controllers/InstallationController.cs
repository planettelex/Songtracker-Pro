using System;
using System.Reflection;
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
        #region Constructor

        public InstallationController(IGetLoginTask getLoginTask,
            IGetInstallationTask getInstallationTask,
            ISeedSystemDataTask seedSystemDataTask) : 
            base(getLoginTask)
        {
            _seedSystemDataTask = seedSystemDataTask;
            _getInstallationTask = getInstallationTask;
        }
        private readonly ISeedSystemDataTask _seedSystemDataTask;
        private readonly IGetInstallationTask _getInstallationTask;
        
        #endregion
        
        [Route(Routes.Root)]
        [HttpGet]
        public IActionResult GetApiInfo()
        {
            try
            {
                var apiInfo = new ApiInfo
                {
                    Version = ApplicationSettings.Version,
                    Documentation = ApplicationSettings.Api.Documentation
                };
                var taskResults = _getInstallationTask.DoTask(null);
                apiInfo.Name = taskResults.Success ? 
                    taskResults.Data.Name : 
                    SeedData("APP_NAME");

                apiInfo.Tagline = taskResults.Success ? 
                    taskResults.Data.Tagline : 
                    SeedData("APP_TAGLINE");

                return Json(apiInfo);
            }
            catch (Exception e)
            {
                return Error(e);
            }
        }

        [Route(Routes.System)]
        [HttpGet]
        [UserTypesAllowed(UserType.SystemAdministrator)]
        public IActionResult GetInstallationInfo()
        {
            try
            {
                if (!UserIsAuthenticatedAndAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                var taskResults = _getInstallationTask.DoTask(null);

                return taskResults.Success ? 
                    Json(taskResults) : 
                    Error(taskResults.Exception);
            }
            catch (Exception e)
            {
                return Error(e);
            }
        }

        [Route(Routes.SystemSeed)]
        [HttpPost]
        [UserTypesAllowed(UserType.SystemAdministrator)]
        public IActionResult SeedSystemData()
        {
            try
            {
                if (!UserIsAuthenticatedAndAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                var taskResults = _seedSystemDataTask.DoTask(null);

                return taskResults.HasException ? 
                    Error(taskResults.Exception) :
                    Json(taskResults.Success);
            }
            catch (Exception e)
            {
                return Error(e);
            }
        }
    }
}
