using System;
using System.Reflection;
using Microsoft.AspNetCore.Http;
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
        public UserController(IGetUserByAuthenticationTokenTask getUserByAuthenticationTokenTask,
                              IListUsersTask listUsersTask,
                              IGetUserTask getUserTask,
                              IAddUserTask addUserTask,
                              IUpdateUserTask updateUserTask,
                              ILoginUserTask loginUserTask,
                              IListUserAccountsTask listUserAccountsTask,
                              IGetUserAccountTask getUserAccountTask,
                              IAddUserAccountTask addUserAccountTask,
                              IUpdateUserAccountTask updateUserAccountTask,
                              IRemoveUserAccountTask removeUserAccountTask,
                              ISendUserInvitationTask sendUserInvitationTask,
                              IGetUserInvitationTask getUserInvitationTask,
                              IAcceptUserInvitationTask acceptUserInvitationTask) :
        base(getUserByAuthenticationTokenTask)
        {
            _listUsersTask = listUsersTask;
            _getUserTask = getUserTask;
            _addUserTask = addUserTask;
            _updateUserTask = updateUserTask;
            _loginUserTask = loginUserTask;
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
        private readonly IListUserAccountsTask _listUserAccountsTask;
        private readonly IGetUserAccountTask _getUserAccountTask;
        private readonly IAddUserAccountTask _addUserAccountTask;
        private readonly IUpdateUserAccountTask _updateUserAccountTask;
        private readonly IRemoveUserAccountTask _removeUserAccountTask;
        private readonly ISendUserInvitationTask _sendUserInvitationTask;
        private readonly IGetUserInvitationTask _getUserInvitationTask;
        private readonly IAcceptUserInvitationTask _acceptUserInvitationTask;

        [Route(Routes.Invitations)]
        [HttpPost]
        [UserTypesAllowed(UserType.SystemAdministrator, UserType.LabelAdministrator, UserType.PublisherAdministrator)]
        public IActionResult InviteUser(UserInvitation userInvitation)
        {
            if (!UserIsAuthorized(MethodBase.GetCurrentMethod()))
                return Unauthorized();

            var taskResults = _sendUserInvitationTask.DoTask(userInvitation);

            if (taskResults.Success)
                return Ok(JsonSerialize(taskResults));

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [Route(Routes.Invitation)]
        [HttpGet]
        public IActionResult GetUserInvitation(Guid uuid)
        {
            var taskResults = _getUserInvitationTask.DoTask(uuid);

            if (taskResults.Success)
                return Ok(JsonSerialize(taskResults));

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [Route(Routes.Invitation)]
        [HttpPost]
        public IActionResult AcceptUserInvitation(Guid uuid, UserInvitation userInvitation)
        {
            userInvitation.Uuid = uuid;
            var taskResults = _acceptUserInvitationTask.DoTask(userInvitation);

            if (taskResults.Success)
                return Ok(JsonSerialize(taskResults));

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [Route(Routes.Login)]
        [HttpPost]
        public IActionResult LoginUser(Login login)
        {
            var taskResults = _loginUserTask.DoTask(login);

            if (taskResults.Success)
                return Ok(JsonSerialize(taskResults));

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [Route(Routes.Users)]
        [HttpPost]
        [UserTypesAllowed(UserType.SystemAdministrator)]
        public IActionResult AddUser(User user)
        {
            if (!UserIsAuthorized(MethodBase.GetCurrentMethod()))
                return Unauthorized();

            var taskResults = _addUserTask.DoTask(user);

            if (taskResults.Success)
                return Ok(JsonSerialize(taskResults));

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [Route(Routes.Users)]
        [HttpGet]
        [UserTypesAllowed(UserType.SystemAdministrator, UserType.LabelAdministrator, UserType.PublisherAdministrator)]
        public IActionResult ListUsers(UserType? type)
        {
            if (!UserIsAuthorized(MethodBase.GetCurrentMethod()))
                return Unauthorized();

            var taskResults = _listUsersTask.DoTask(type);

            if (!taskResults.Success) 
                return StatusCode(StatusCodes.Status500InternalServerError);

            foreach (var user in taskResults.Data)
                user.AuthenticationToken = null;

            return Ok(JsonSerialize(taskResults));
        }

        [Route(Routes.User)]
        [HttpGet]
        [UserTypesAllowed(UserType.Unassigned)]
        public IActionResult GetUser(int id)
        {
            if (!UserIsAuthorized(MethodBase.GetCurrentMethod()))
                return Unauthorized();

            if (AuthenticatedUser.Type == UserType.SystemUser && AuthenticatedUser.Id != id)
                return Unauthorized();

            var taskResults = _getUserTask.DoTask(id);

            if (!taskResults.Success) 
                return StatusCode(StatusCodes.Status500InternalServerError);

            taskResults.Data.AuthenticationToken = null;
            return Ok(JsonSerialize(taskResults));
        }

        [Route(Routes.User)]
        [HttpPut]
        [UserTypesAllowed(UserType.Unassigned)]
        public IActionResult UpdateUser(int id, User user)
        {
            if (!UserIsAuthorized(MethodBase.GetCurrentMethod()))
                return Unauthorized();

            if (AuthenticatedUser.Type == UserType.SystemUser && AuthenticatedUser.Id != id)
                return Unauthorized();

            user.Id = id;
            var taskResults = _updateUserTask.DoTask(user);

            if (taskResults.Success)
                return Ok();

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [Route(Routes.UserAccounts)]
        [HttpPost]
        [UserTypesAllowed(UserType.Unassigned)]
        public IActionResult AddUserAccount(int userId, UserAccount userAccount)
        {
            if (!UserIsAuthorized(MethodBase.GetCurrentMethod()))
                return Unauthorized();

            if (AuthenticatedUser.Type == UserType.SystemUser && AuthenticatedUser.Id != userId)
                return Unauthorized();

            userAccount.UserId = userId;
            var taskResults = _addUserAccountTask.DoTask(userAccount);

            if (taskResults.Success)
                return Ok(JsonSerialize(taskResults));

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [Route(Routes.UserAccounts)]
        [HttpGet]
        [UserTypesAllowed(UserType.Unassigned)]
        public IActionResult ListUserAccounts(int userId)
        {
            if (!UserIsAuthorized(MethodBase.GetCurrentMethod()))
                return Unauthorized();

            if (AuthenticatedUser.Type == UserType.SystemUser && AuthenticatedUser.Id != userId)
                return Unauthorized();

            var user = _getUserTask.DoTask(userId).Data;
            var taskResults = _listUserAccountsTask.DoTask(user);

            if (taskResults.Success)
                return Ok(JsonSerialize(taskResults));

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [Route(Routes.UserAccount)]
        [HttpPut]
        [UserTypesAllowed(UserType.Unassigned)]
        public IActionResult UpdateUserAccount(int userId, int userAccountId, UserAccount userAccount)
        {
            if (!UserIsAuthorized(MethodBase.GetCurrentMethod()))
                return Unauthorized();

            if (AuthenticatedUser.Type == UserType.SystemUser && AuthenticatedUser.Id != userId)
                return Unauthorized();

            userAccount.UserId = userId;
            userAccount.Id = userAccountId;
            var taskResults = _updateUserAccountTask.DoTask(userAccount);

            if (taskResults.Success)
                return Ok();

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [Route(Routes.UserAccount)]
        [HttpDelete]
        [UserTypesAllowed(UserType.Unassigned)]
        public IActionResult RemoveUserAccount(int userId, int userAccountId)
        {
            if (!UserIsAuthorized(MethodBase.GetCurrentMethod()))
                return Unauthorized();

            if (AuthenticatedUser.Type == UserType.SystemUser && AuthenticatedUser.Id != userId)
                return Unauthorized();

            var toRemove = _getUserAccountTask.DoTask(userAccountId).Data;
            if (toRemove == null)
                return Ok(JsonSerialize(false));

            if (toRemove.UserId == userId)
            {
                var taskResults = _removeUserAccountTask.DoTask(toRemove);
                if (taskResults.Success)
                    return Ok(JsonSerialize(true));
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}
