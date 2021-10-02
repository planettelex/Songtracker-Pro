using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        #region Constructor

        public PublishingController(IGetLoginTask getLoginTask,
                                    IListPerformingRightsOrganizationsTask listPerformingRightsOrganizationsTask, 
                                    IListPublishersTask listPublishersTask,
                                    IGetPublisherTask getPublisherTask,
                                    IAddPublisherTask addPublisherTask,
                                    IUpdatePublisherTask updatePublisherTask,
                                    ILogger<PublishingController> logger) :
        base (getLoginTask, logger)
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

        #endregion

        [Route(Routes.PerformingRightsOrganizations)]
        [HttpGet]
        [UserTypesAllowed(UserType.Unassigned)]
        public IActionResult ListPerformingRightsOrganizations()
        {
            try
            {
                if (!ClientKeyIsValid())
                    return Unauthorized();

                if (!UserIsAuthenticatedAndAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                var taskResults = _listPerformingRightsOrganizationsTask.DoTask(null);

                return taskResults.Success ? 
                    Json(taskResults) : 
                    Error(taskResults.Exception);
            }
            catch (Exception e)
            {
                return Error(e);
            }
        }

        [Route(Routes.Publishers)]
        [HttpPost]
        [UserTypesAllowed(UserType.SystemAdministrator)]
        public IActionResult AddPublisher(Publisher publisher)
        {
            try
            {
                if (!ClientKeyIsValid())
                    return Unauthorized();

                if (!UserIsAuthenticatedAndAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                var taskResults = _addPublisherTask.DoTask(publisher);

                return taskResults.Success ? 
                    Json(taskResults) : 
                    Error(taskResults.Exception);
            }
            catch (Exception e)
            {
                return Error(e);
            }
        }

        [Route(Routes.Publishers)]
        [HttpGet]
        [UserTypesAllowed(UserType.SystemAdministrator)]
        public IActionResult ListPublishers()
        {
            try
            {
                if (!ClientKeyIsValid())
                    return Unauthorized();

                if (!UserIsAuthenticatedAndAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                var taskResults = _listPublishersTask.DoTask(null);

                return taskResults.Success ? 
                    Json(taskResults) : 
                    Error(taskResults.Exception);
            }
            catch (Exception e)
            {
                return Error(e);
            }
        }

        [Route(Routes.Publisher)]
        [HttpGet]
        [UserTypesAllowed(UserType.Unassigned)]
        public IActionResult GetPublisher(int id)
        {
            try
            {
                if (!ClientKeyIsValid())
                    return Unauthorized();

                if (!UserIsAuthenticatedAndAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                var taskResults = _getPublisherTask.DoTask(id);

                if (taskResults.HasException)
                    return Error(taskResults.Exception);

                if (taskResults.HasNoData)
                    return NotFound();

                var userIsPublisherAdmin = AuthenticatedUser.Type == UserType.PublisherAdministrator && AuthenticatedUser.Publisher?.Id != id;
                var allowedToSeeSensitiveData = AuthenticatedUser.Type == UserType.SystemAdministrator || userIsPublisherAdmin;
                if (!allowedToSeeSensitiveData)
                    taskResults.Data.TaxId = null;

                return Json(taskResults);
            }
            catch (Exception e)
            {
                return Error(e);
            }
        }

        [Route(Routes.Publisher)]
        [HttpPut]
        [UserTypesAllowed(UserType.SystemAdministrator, UserType.PublisherAdministrator)]
        public IActionResult UpdatePublisher(int id, Publisher publisher)
        {
            try
            {
                if (!ClientKeyIsValid())
                    return Unauthorized();

                if (!UserIsAuthenticatedAndAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                if (AuthenticatedUser.Type == UserType.PublisherAdministrator && AuthenticatedUser.Publisher?.Id != id)
                    return Unauthorized();

                var invalidPublisherPathResult = InvalidPublisherPathResult(id);
                if (invalidPublisherPathResult != null)
                    return invalidPublisherPathResult;

                publisher.Id = id;
                var taskResults = _updatePublisherTask.DoTask(publisher);

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

        private IActionResult InvalidPublisherPathResult(int publisherId)
        {
            var getPublisherResult = _getPublisherTask.DoTask(publisherId);

            if (getPublisherResult.HasException)
                return Error(getPublisherResult.Exception);

            if (getPublisherResult.HasNoData)
                return NotFound();

            return null;
        }

        #endregion
    }
}
