using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using SongtrackerPro.Api.Attributes;
using SongtrackerPro.Data.Enums;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Tasks.UserTasks;

namespace SongtrackerPro.Api.Controllers
{
    [ApiController]
    public class UserController : ApiControllerBase
    {
        #region Constructor

        public UserController(IGetLoginTask getLoginTask,
            IListUsersTask listUsersTask,
            IGetUserTask getUserTask,
            IAddUserTask addUserTask,
            IUpdateUserTask updateUserTask,
            ILoginUserTask loginUserTask,
            ILogoutUserTask logoutUserTask,
            IListUserAccountsTask listUserAccountsTask,
            IGetUserAccountTask getUserAccountTask,
            IAddUserAccountTask addUserAccountTask,
            IUpdateUserAccountTask updateUserAccountTask,
            IRemoveUserAccountTask removeUserAccountTask,
            ISendUserInvitationTask sendUserInvitationTask,
            IGetUserInvitationTask getUserInvitationTask,
            IAcceptUserInvitationTask acceptUserInvitationTask) :
            base(getLoginTask)
        {
            _listUsersTask = listUsersTask;
            _getUserTask = getUserTask;
            _addUserTask = addUserTask;
            _updateUserTask = updateUserTask;
            _loginUserTask = loginUserTask;
            _logoutUserTask = logoutUserTask;
            _listUserAccountsTask = listUserAccountsTask;
            _getUserAccountTask = getUserAccountTask;
            _addUserAccountTask = addUserAccountTask;
            _updateUserAccountTask = updateUserAccountTask;
            _removeUserAccountTask = removeUserAccountTask;
            _sendUserInvitationTask = sendUserInvitationTask;
            _getUserInvitationTask = getUserInvitationTask;
            _acceptUserInvitationTask = acceptUserInvitationTask;
        }
        private readonly IListUsersTask _listUsersTask;
        private readonly IGetUserTask _getUserTask;
        private readonly IAddUserTask _addUserTask;
        private readonly IUpdateUserTask _updateUserTask;
        private readonly ILoginUserTask _loginUserTask;
        private readonly ILogoutUserTask _logoutUserTask;
        private readonly IListUserAccountsTask _listUserAccountsTask;
        private readonly IGetUserAccountTask _getUserAccountTask;
        private readonly IAddUserAccountTask _addUserAccountTask;
        private readonly IUpdateUserAccountTask _updateUserAccountTask;
        private readonly IRemoveUserAccountTask _removeUserAccountTask;
        private readonly ISendUserInvitationTask _sendUserInvitationTask;
        private readonly IGetUserInvitationTask _getUserInvitationTask;
        private readonly IAcceptUserInvitationTask _acceptUserInvitationTask;

        #endregion

        [Route(Routes.Invitations)]
        [HttpPost]
        [UserTypesAllowed(UserType.SystemAdministrator, UserType.LabelAdministrator, UserType.PublisherAdministrator)]
        public IActionResult InviteUser(UserInvitation userInvitation)
        {
            try
            {
                if (!UserIsAuthorized(MethodBase.GetCurrentMethod()))
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

        [Route(Routes.Invitation)]
        [HttpGet]
        public IActionResult GetUserInvitation(Guid uuid)
        {
            try
            {
                var taskResults = _getUserInvitationTask.DoTask(uuid);

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
        [HttpPost]
        public IActionResult AcceptUserInvitation(Guid uuid, UserInvitation userInvitation)
        {
            try
            {
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

        [Route(Routes.Login)]
        [HttpPost]
        public IActionResult LoginUser(Login login)
        {
            try
            {
                if (login == null || string.IsNullOrEmpty(login.AuthenticationToken))
                    return BadRequest();
            
                var taskResults = _loginUserTask.DoTask(login);

                return taskResults.Success ? 
                    Json(taskResults) : 
                    Error(taskResults.Exception);
            }
            catch (Exception e)
            {
                return Error(e);
            }
        }

        [Route(Routes.Logout)]
        [HttpPost]
        public IActionResult LogoutUser()
        {
            try
            {
                if (AuthenticatedUser == null)
                    return NoContent();

                var taskResults = _logoutUserTask.DoTask(AuthenticatedUser);

                return taskResults.Success ? 
                    Ok() : 
                    Error(taskResults.Exception);
            }
            catch (Exception e)
            {
                return Error(e);
            }
        }

        [Route(Routes.Users)]
        [HttpPost]
        [UserTypesAllowed(UserType.SystemAdministrator)]
        public IActionResult AddUser(User user)
        {
            try
            {
                if (!UserIsAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                var taskResults = _addUserTask.DoTask(user);

                return taskResults.Success ? 
                    Json(taskResults) : 
                    Error(taskResults.Exception);
            }
            catch (Exception e)
            {
                return Error(e);
            }
        }

        [Route(Routes.Users)]
        [HttpGet]
        [UserTypesAllowed(UserType.SystemAdministrator, UserType.LabelAdministrator, UserType.PublisherAdministrator)]
        public IActionResult ListUsers(UserType? type)
        {
            try
            {
                if (!UserIsAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                var taskResults = _listUsersTask.DoTask(type);

                return taskResults.Success ? 
                    Json(taskResults) : 
                    Error(taskResults.Exception);
            }
            catch (Exception e)
            {
                return Error(e);
            }
        }

        [Route(Routes.User)]
        [HttpGet]
        [UserTypesAllowed(UserType.Unassigned)]
        public IActionResult GetUser(int id)
        {
            try
            {
                if (!UserIsAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                if (AuthenticatedUser.Type == UserType.SystemUser && AuthenticatedUser.Id != id)
                    return Unauthorized();

                var taskResults = _getUserTask.DoTask(id);

                return taskResults.Success ? 
                    Json(taskResults) : 
                    Error(taskResults.Exception);
            }
            catch (Exception e)
            {
                return Error(e);
            }
        }

        [Route(Routes.User)]
        [HttpPut]
        [UserTypesAllowed(UserType.Unassigned)]
        public IActionResult UpdateUser(int id, User user)
        {
            try
            {
                if (!UserIsAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                if (AuthenticatedUser.Type == UserType.SystemUser && AuthenticatedUser.Id != id)
                    return Unauthorized();

                user.Id = id;
                var taskResults = _updateUserTask.DoTask(user);

                return taskResults.Success ? 
                    Ok() : 
                    Error(taskResults.Exception);
            }
            catch (Exception e)
            {
                return Error(e);
            }
        }

        [Route(Routes.UserAccounts)]
        [HttpPost]
        [UserTypesAllowed(UserType.Unassigned)]
        public IActionResult AddUserAccount(int userId, UserAccount userAccount)
        {
            try
            {
                if (!UserIsAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                if (AuthenticatedUser.Type == UserType.SystemUser && AuthenticatedUser.Id != userId)
                    return Unauthorized();

                userAccount.UserId = userId;
                var taskResults = _addUserAccountTask.DoTask(userAccount);

                return taskResults.Success ? 
                    Json(taskResults) : 
                    Error(taskResults.Exception);
            }
            catch (Exception e)
            {
                return Error(e);
            }
        }

        [Route(Routes.UserAccounts)]
        [HttpGet]
        [UserTypesAllowed(UserType.Unassigned)]
        public IActionResult ListUserAccounts(int userId)
        {
            try
            {
                if (!UserIsAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                if (AuthenticatedUser.Type == UserType.SystemUser && AuthenticatedUser.Id != userId)
                    return Unauthorized();

                var user = _getUserTask.DoTask(userId).Data;
                var taskResults = _listUserAccountsTask.DoTask(user);

                return taskResults.Success ? 
                    Json(taskResults) : 
                    Error(taskResults.Exception);
            }
            catch (Exception e)
            {
                return Error(e);
            }
        }

        [Route(Routes.UserAccount)]
        [HttpPut]
        [UserTypesAllowed(UserType.Unassigned)]
        public IActionResult UpdateUserAccount(int userId, int userAccountId, UserAccount userAccount)
        {
            try
            {
                if (!UserIsAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                if (AuthenticatedUser.Type == UserType.SystemUser && AuthenticatedUser.Id != userId)
                    return Unauthorized();

                userAccount.UserId = userId;
                userAccount.Id = userAccountId;
                var taskResults = _updateUserAccountTask.DoTask(userAccount);

                return taskResults.Success ? 
                    Ok() : 
                    Error(taskResults.Exception);
            }
            catch (Exception e)
            {
                return Error(e);
            }
        }

        [Route(Routes.UserAccount)]
        [HttpDelete]
        [UserTypesAllowed(UserType.Unassigned)]
        public IActionResult RemoveUserAccount(int userId, int userAccountId)
        {
            try
            {
                if (!UserIsAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                if (AuthenticatedUser.Type == UserType.SystemUser && AuthenticatedUser.Id != userId)
                    return Unauthorized();

                var getUserTaskResult = _getUserAccountTask.DoTask(userAccountId);
                if (!getUserTaskResult.Success) 
                    return Error(getUserTaskResult.Exception);

                var toRemove = getUserTaskResult.Data;
                if (toRemove == null)
                    return Json(false);

                if (toRemove.UserId != userId)
                    return BadRequest();

                var taskResults = _removeUserAccountTask.DoTask(toRemove);

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
