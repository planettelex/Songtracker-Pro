using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SongtrackerPro.Api.Attributes;
using SongtrackerPro.Data.Enums;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Tasks.RecordLabelTasks;
using SongtrackerPro.Tasks.UserTasks;

namespace SongtrackerPro.Api.Controllers
{
    [ApiController]
    public class RecordLabelController : ApiControllerBase
    {
        #region Constructor

        public RecordLabelController(IGetLoginTask getLoginTask,
                                     IListRecordLabelsTask listRecordLabelsTask,
                                     IGetRecordLabelTask getRecordLabelTask,
                                     IAddRecordLabelTask addRecordLabelTask,
                                     IUpdateRecordLabelTask updateRecordLabelTask,
                                     ILogger<RecordLabelController> logger) :
        base(getLoginTask, logger)
        {
            _listRecordLabelsTask = listRecordLabelsTask;
            _getRecordLabelTask = getRecordLabelTask;
            _addRecordLabelTask = addRecordLabelTask;
            _updateRecordLabelTask = updateRecordLabelTask;
        }
        private readonly IListRecordLabelsTask _listRecordLabelsTask;
        private readonly IGetRecordLabelTask _getRecordLabelTask;
        private readonly IAddRecordLabelTask _addRecordLabelTask;
        private readonly IUpdateRecordLabelTask _updateRecordLabelTask;

        #endregion

        [Route(Routes.RecordLabels)]
        [HttpPost]
        [UserTypesAllowed(UserType.SystemAdministrator)]
        public IActionResult AddRecordLabel(RecordLabel recordLabel)
        {
            try
            {
                if (!ClientKeyIsValid())
                    return Unauthorized();

                if (!UserIsAuthenticatedAndAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                var taskResults = _addRecordLabelTask.DoTask(recordLabel);

                return taskResults.Success ? 
                    Json(taskResults) : 
                    Error(taskResults.Exception);
            }
            catch (Exception e)
            {
                return Error(e);
            }
        }

        [Route(Routes.RecordLabels)]
        [HttpGet]
        [UserTypesAllowed(UserType.SystemAdministrator)]
        public IActionResult ListRecordLabels()
        {
            try
            {
                if (!ClientKeyIsValid())
                    return Unauthorized();

                if (!UserIsAuthenticatedAndAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                var taskResults = _listRecordLabelsTask.DoTask(null);

                return taskResults.Success ? 
                    Json(taskResults) : 
                    Error(taskResults.Exception);
            }
            catch (Exception e)
            {
                return Error(e);
            }
        }

        [Route(Routes.RecordLabel)]
        [HttpGet]
        [UserTypesAllowed(UserType.Unassigned)]
        [SystemUserRolesAllowed(SystemUserRoles.All)]
        public IActionResult GetRecordLabel(int id)
        {
            try
            {
                if (!ClientKeyIsValid())
                    return Unauthorized();

                if (!UserIsAuthenticatedAndAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                var taskResults = _getRecordLabelTask.DoTask(id);

                if (taskResults.HasException)
                    return Error(taskResults.Exception);

                if (taskResults.HasNoData)
                    return NotFound();

                var userIsLabelAdmin = AuthenticatedUser.Type == UserType.LabelAdministrator && AuthenticatedUser.RecordLabel?.Id == id;
                var allowedToSeeSensitiveData = AuthenticatedUser.Type == UserType.SystemAdministrator || userIsLabelAdmin;
                if (!allowedToSeeSensitiveData)
                    taskResults.Data.TaxId = null;

                return Json(taskResults);
            }
            catch (Exception e)
            {
                return Error(e);
            }
        }

        [Route(Routes.RecordLabel)]
        [HttpPut]
        [UserTypesAllowed(UserType.SystemAdministrator, UserType.LabelAdministrator)]
        public IActionResult UpdateRecordLabel(int id, RecordLabel recordLabel)
        {
            try
            {
                if (!ClientKeyIsValid())
                    return Unauthorized();

                if (!UserIsAuthenticatedAndAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                if (AuthenticatedUser.Type == UserType.LabelAdministrator && AuthenticatedUser.RecordLabel?.Id != id)
                    return Unauthorized();

                var invalidRecordLabelPathResult = InvalidRecordLabelPathResult(id);
                if (invalidRecordLabelPathResult != null)
                    return invalidRecordLabelPathResult;

                recordLabel.Id = id;
                var taskResults =_updateRecordLabelTask.DoTask(recordLabel);

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

        private IActionResult InvalidRecordLabelPathResult(int recordLabelId)
        {
            var getRecordLabelResult = _getRecordLabelTask.DoTask(recordLabelId);

            if (getRecordLabelResult.HasException)
                return Error(getRecordLabelResult.Exception);

            if (getRecordLabelResult.HasNoData)
                return NotFound();

            return null;
        }

        #endregion
    }
}
