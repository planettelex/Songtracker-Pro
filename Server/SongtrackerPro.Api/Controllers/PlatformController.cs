using System;
using System.Reflection;
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
        #region Constructor

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

        #endregion
        
        [Route(Routes.Services)]
        [HttpGet]
        [UserTypesAllowed(UserType.Unassigned)]
        public IActionResult ListServices()
        {
            try
            {
                if (!ClientKeyIsValid())
                    return Unauthorized();

                if (!UserIsAuthenticatedAndAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                var taskResults = _listServicesTask.DoTask(null);

                return taskResults.Success ? 
                    Json(taskResults) : 
                    Error(taskResults.Exception);
            }
            catch (Exception e)
            {
                return Error(e);
            }
        }

        [Route(Routes.Platforms)]
        [HttpPost]
        [UserTypesAllowed(UserType.SystemAdministrator)]
        public IActionResult AddPlatform(Platform platform)
        {
            try
            {
                if (!ClientKeyIsValid())
                    return Unauthorized();

                if (!UserIsAuthenticatedAndAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                var taskResults = _addPlatformTask.DoTask(platform);

                return taskResults.Success ? 
                    Json(taskResults) : 
                    Error(taskResults.Exception);
            }
            catch (Exception e)
            {
                return Error(e);
            }
        }

        [Route(Routes.Platforms)]
        [HttpGet]
        [UserTypesAllowed(UserType.Unassigned)]
        public IActionResult ListPlatforms()
        {
            try
            {
                if (!ClientKeyIsValid())
                    return Unauthorized();

                if (!UserIsAuthenticatedAndAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                var taskResults = _listPlatformsTask.DoTask(null);

                return taskResults.Success ? 
                    Json(taskResults) : 
                    Error(taskResults.Exception);
            }
            catch (Exception e)
            {
                return Error(e);
            }
        }

        [Route(Routes.Platform)]
        [HttpGet]
        [UserTypesAllowed(UserType.Unassigned)]
        public IActionResult GetPlatform(int id)
        {
            try
            {
                if (!ClientKeyIsValid())
                    return Unauthorized();

                if (!UserIsAuthenticatedAndAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                var taskResults = _getPlatformTask.DoTask(id);

                if (taskResults.HasException)
                    return Error(taskResults.Exception);

                if (taskResults.HasNoData)
                    return NotFound();

                return Json(taskResults);
            }
            catch (Exception e)
            {
                return Error(e);
            }
        }

        [Route(Routes.Platform)]
        [HttpPut]
        [UserTypesAllowed(UserType.SystemAdministrator)]
        public IActionResult UpdatePlatform(int id, Platform platform)
        {
            try
            {
                if (!ClientKeyIsValid())
                    return Unauthorized();

                if (!UserIsAuthenticatedAndAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                var invalidPlatformPathResult = InvalidPlatformPathResult(id);
                if (invalidPlatformPathResult != null)
                    return invalidPlatformPathResult;

                platform.Id = id;
                var taskResults = _updatePlatformTask.DoTask(platform);

                return taskResults.Success ? 
                    Ok() : 
                    Error(taskResults.Exception);
            }
            catch (Exception e)
            {
                return Error(e);
            }
        }

        #region Private

        private IActionResult InvalidPlatformPathResult(int platformId)
        {
            var getPlatformResult = _getPlatformTask.DoTask(platformId);

            if (getPlatformResult.HasException)
                return Error(getPlatformResult.Exception);

            if (getPlatformResult.HasNoData)
                return NotFound();

            return null;
        }

        #endregion
    }
}
