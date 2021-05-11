using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public RecordLabelController(IGetLoginTask getLoginTask,
                                     IListRecordLabelsTask listRecordLabelsTask,
                                     IGetRecordLabelTask getRecordLabelTask,
                                     IAddRecordLabelTask addRecordLabelTask,
                                     IUpdateRecordLabelTask updateRecordLabelTask) :
        base(getLoginTask)
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

        [Route(Routes.RecordLabels)]
        [HttpPost]
        [UserTypesAllowed(UserType.SystemAdministrator)]
        public IActionResult AddRecordLabel(RecordLabel recordLabel)
        {
            if (!UserIsAuthorized(MethodBase.GetCurrentMethod()))
                return Unauthorized();

            var taskResults = _addRecordLabelTask.DoTask(recordLabel);

            if (taskResults.Success)
                return Ok(JsonSerialize(taskResults));

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [Route(Routes.RecordLabels)]
        [HttpGet]
        [UserTypesAllowed(UserType.SystemAdministrator)]
        public IActionResult ListRecordLabels()
        {
            if (!UserIsAuthorized(MethodBase.GetCurrentMethod()))
                return Unauthorized();

            var taskResults = _listRecordLabelsTask.DoTask(null);

            if (taskResults.Success)
                return Ok(JsonSerialize(taskResults));

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [Route(Routes.RecordLabel)]
        [HttpGet]
        [UserTypesAllowed(UserType.Unassigned)]
        [SystemUserRolesAllowed(SystemUserRoles.All)]
        public IActionResult GetRecordLabel(int id)
        {
            if (!UserIsAuthorized(MethodBase.GetCurrentMethod()))
                return Unauthorized();

            var taskResults = _getRecordLabelTask.DoTask(id);

            if (taskResults.Success)
            {
                var userIsLabelAdmin = AuthenticatedUser.Type == UserType.LabelAdministrator && AuthenticatedUser.RecordLabel?.Id == id;
                var allowedToSeeSensitiveData = AuthenticatedUser.Type == UserType.SystemAdministrator || userIsLabelAdmin;
                if (!allowedToSeeSensitiveData)
                    taskResults.Data.TaxId = null;

                return Ok(JsonSerialize(taskResults));
            }
            
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [Route(Routes.RecordLabel)]
        [HttpPut]
        [UserTypesAllowed(UserType.SystemAdministrator, UserType.LabelAdministrator)]
        public IActionResult UpdateRecordLabel(int id, RecordLabel recordLabel)
        {
            if (!UserIsAuthorized(MethodBase.GetCurrentMethod()))
                return Unauthorized();

            if (AuthenticatedUser.Type == UserType.LabelAdministrator && AuthenticatedUser.RecordLabel?.Id != id)
                return Unauthorized();

            recordLabel.Id = id;
            var taskResults =_updateRecordLabelTask.DoTask(recordLabel);

            if (taskResults.Success)
                return Ok();

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
