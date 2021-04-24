using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SongtrackerPro.Api.Attributes;
using SongtrackerPro.Data.Enums;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Tasks.PublishingTasks;
using SongtrackerPro.Tasks.UserTasks;

namespace SongtrackerPro.Api.Controllers
{
    [ApiController]
    public class PublishingController : ApiControllerBase
    {
        public PublishingController(IGetUserByAuthenticationTokenTask getUserByAuthenticationTokenTask,
                                    IListPerformingRightsOrganizationsTask listPerformingRightsOrganizationsTask, 
                                    IListPublishersTask listPublishersTask,
                                    IGetPublisherTask getPublisherTask,
                                    IAddPublisherTask addPublisherTask,
                                    IUpdatePublisherTask updatePublisherTask) :
        base (getUserByAuthenticationTokenTask)
        {
            _listPerformingRightsOrganizationsTask = listPerformingRightsOrganizationsTask;
            _listPublishersTask = listPublishersTask;
            _getPublisherTask = getPublisherTask;
            _addPublisherTask = addPublisherTask;
            _updatePublisherTask = updatePublisherTask;
        }
        private readonly IListPerformingRightsOrganizationsTask _listPerformingRightsOrganizationsTask;
        private readonly IListPublishersTask _listPublishersTask;
        private readonly IGetPublisherTask _getPublisherTask;
        private readonly IAddPublisherTask _addPublisherTask;
        private readonly IUpdatePublisherTask _updatePublisherTask;

        [Route(Routes.PerformingRightsOrganizations)]
        [HttpGet]
        [UserTypesAllowed(UserType.Unassigned)]
        public IActionResult ListPerformingRightsOrganizations()
        {
            if (!UserIsAuthorized(MethodBase.GetCurrentMethod()))
                return Unauthorized();

            var taskResults = _listPerformingRightsOrganizationsTask.DoTask(null);

            if (taskResults.Success)
                return Ok(JsonSerialize(taskResults));

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [Route(Routes.Publishers)]
        [HttpPost]
        [UserTypesAllowed(UserType.SystemAdministrator)]
        public IActionResult AddPublisher(Publisher publisher)
        {
            if (!UserIsAuthorized(MethodBase.GetCurrentMethod()))
                return Unauthorized();

            var taskResults = _addPublisherTask.DoTask(publisher);

            if (taskResults.Success)
                return Ok(JsonSerialize(taskResults));

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [Route(Routes.Publishers)]
        [HttpGet]
        [UserTypesAllowed(UserType.SystemAdministrator)]
        public IActionResult ListPublishers()
        {
            if (!UserIsAuthorized(MethodBase.GetCurrentMethod()))
                return Unauthorized();

            var taskResults = _listPublishersTask.DoTask(null);

            if (taskResults.Success)
                return Ok(JsonSerialize(taskResults));

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [Route(Routes.Publisher)]
        [HttpGet]
        [UserTypesAllowed(UserType.Unassigned)]
        public IActionResult GetPublisher(int id)
        {
            if (!UserIsAuthorized(MethodBase.GetCurrentMethod()))
                return Unauthorized();

            var taskResults = _getPublisherTask.DoTask(id);

            if (taskResults.Success)
            {
                var userIsPublisherAdmin = AuthenticatedUser.Type == UserType.PublisherAdministrator && AuthenticatedUser.Publisher?.Id != id;
                var allowedToSeeSensitiveData = AuthenticatedUser.Type == UserType.SystemAdministrator || userIsPublisherAdmin;
                if (!allowedToSeeSensitiveData)
                    taskResults.Data.TaxId = null;

                return Ok(JsonSerialize(taskResults));
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [Route(Routes.Publisher)]
        [HttpPut]
        [UserTypesAllowed(UserType.SystemAdministrator, UserType.PublisherAdministrator)]
        public IActionResult UpdatePublisher(int id, Publisher publisher)
        {
            if (!UserIsAuthorized(MethodBase.GetCurrentMethod()))
                return Unauthorized();

            if (AuthenticatedUser.Type == UserType.PublisherAdministrator && AuthenticatedUser.Publisher?.Id != id)
                return Unauthorized();

            publisher.Id = id;
            var taskResults = _updatePublisherTask.DoTask(publisher);

            if (taskResults.Success)
                return Ok();

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
