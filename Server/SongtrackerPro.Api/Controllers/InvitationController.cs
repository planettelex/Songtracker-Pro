using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SongtrackerPro.Api.Attributes;
using SongtrackerPro.Data.Enums;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Tasks.UserTasks;

namespace SongtrackerPro.Api.Controllers
{
    [ApiController]
    public class InvitationController : ApiControllerBase
    {
        #region Constructor

        public InvitationController(IGetLoginTask getLoginTask,
                                    ISendUserInvitationTask sendUserInvitationTask,
                                    IResendUserInvitationTask resendUserInvitationTask,
                                    IListUserInvitationsTask listUserInvitationsTask,
                                    IGetUserInvitationTask getUserInvitationTask,
                                    IAcceptUserInvitationTask acceptUserInvitationTask,
                                    IRemoveUserInvitationTask removeUserInvitationTask,
                                    ILogger<InvitationController> logger) : 
        base(getLoginTask, logger)
        {
            _sendUserInvitationTask = sendUserInvitationTask;
            _resendUserInvitationTask = resendUserInvitationTask;
            _listUserInvitationsTask = listUserInvitationsTask;
            _getUserInvitationTask = getUserInvitationTask;
            _acceptUserInvitationTask = acceptUserInvitationTask;
            _removeUserInvitationTask = removeUserInvitationTask;
        }
        private readonly ISendUserInvitationTask _sendUserInvitationTask;
        private readonly IResendUserInvitationTask _resendUserInvitationTask;
        private readonly IListUserInvitationsTask _listUserInvitationsTask;
        private readonly IGetUserInvitationTask _getUserInvitationTask;
        private readonly IAcceptUserInvitationTask _acceptUserInvitationTask;
        private readonly IRemoveUserInvitationTask _removeUserInvitationTask;

        #endregion

        [Route(Routes.Invitations)]
        [HttpPost]
        [UserTypesAllowed(UserType.SystemAdministrator, UserType.LabelAdministrator, UserType.PublisherAdministrator)]
        public IActionResult InviteUser(UserInvitation userInvitation)
        {
            try
            {
                if (!ClientKeyIsValid())
                    return Unauthorized();

                if (!UserIsAuthenticatedAndAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                var taskResults = _sendUserInvitationTask.DoTask(userInvitation);

                return taskResults.Success ? 
                    Json(taskResults) : 
                    Error(taskResults.Exception);
            }
            catch (Exception e)
            {
                return Error(e);
            }
        }

        [Route(Routes.Invitations)]
        [HttpGet]
        [UserTypesAllowed(UserType.SystemAdministrator, UserType.LabelAdministrator, UserType.PublisherAdministrator)]
        public IActionResult ListInvitations()
        {
            try
            {
                if (!ClientKeyIsValid())
                    return Unauthorized();

                if (!UserIsAuthenticatedAndAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                var taskResults = _listUserInvitationsTask.DoTask(null);

                return taskResults.Success ? 
                    Json(taskResults) : 
                    Error(taskResults.Exception);
            }
            catch (Exception e)
            {
                return Error(e);
            }
        }

        [Route(Routes.Invitation)]
        [HttpGet]
        [UserTypesAllowed(UserType.Unassigned)]
        public IActionResult GetInvitation(Guid uuid)
        {
            try
            {
                if (!ClientKeyIsValid())
                    return Unauthorized();

                var taskResults = _getUserInvitationTask.DoTask(uuid);

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

        [Route(Routes.Invitation)]
        [HttpPost]
        [UserTypesAllowed(UserType.SystemAdministrator, UserType.LabelAdministrator, UserType.PublisherAdministrator)]
        public IActionResult ResendInvitation(Guid uuid)
        {
            try
            {
                if (!ClientKeyIsValid())
                    return Unauthorized();

                if (!UserIsAuthenticatedAndAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                var taskResults = _resendUserInvitationTask.DoTask(uuid);

                return taskResults.Success ? 
                    Json(taskResults) : 
                    Error(taskResults.Exception);
            }
            catch (Exception e)
            {
                return Error(e);
            }
        }

        [Route(Routes.Invitation)]
        [HttpPut]
        [UserTypesAllowed(UserType.Unassigned)]
        public IActionResult AcceptInvitation(Guid uuid, UserInvitation userInvitation)
        {
            try
            {
                if (!ClientKeyIsValid())
                    return Unauthorized();

                var getUserInvitationResults = _getUserInvitationTask.DoTask(uuid);

                if (getUserInvitationResults.HasException)
                    return Error(getUserInvitationResults.Exception);

                if (getUserInvitationResults.HasNoData)
                    return NotFound();

                userInvitation.Uuid = uuid;
                var taskResults = _acceptUserInvitationTask.DoTask(userInvitation);

                return taskResults.Success ? 
                    Json(taskResults) : 
                    Error(taskResults.Exception);
            }
            catch (Exception e)
            {
                return Error(e);
            }
        }

        [Route(Routes.Invitation)]
        [HttpDelete]
        [UserTypesAllowed(UserType.SystemAdministrator, UserType.LabelAdministrator, UserType.PublisherAdministrator)]
        public IActionResult DeleteInvitation(Guid uuid)
        {
            try
            {
                if (!ClientKeyIsValid())
                    return Unauthorized();

                if (!UserIsAuthenticatedAndAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                var taskResults = _removeUserInvitationTask.DoTask(uuid);

                return taskResults.Success ? 
                    Json(true) : 
                    Error(taskResults.Exception);
            }
            catch (Exception e)
            {
                return Error(e);
            }
        }
    }
}
