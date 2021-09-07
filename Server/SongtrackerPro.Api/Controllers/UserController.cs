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
            IRemoveUserAccountTask removeUserAccountTask) :
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

        #endregion

        [Route(Routes.Login)]
        [HttpPost]
        [UserTypesAllowed(UserType.Unassigned)]
        public IActionResult LoginUser(Login login)
        {
            try
            {
                if (!ClientKeyIsValid())
                    return Unauthorized();

                if (login == null || string.IsNullOrEmpty(login.AuthenticationToken))
                    return BadRequest();

                var taskResults = _loginUserTask.DoTask(login);

                return taskResults.Success ? 
                    Json(taskResults) : 
                        taskResults.Exception.InnerException != null && 
                        taskResults.Exception.InnerException.Message == SystemMessage("USER_NOT_FOUND") ?
                    NotFound() :
                    Error(taskResults.Exception);
            }
            catch (Exception e)
            {
                return Error(e);
            }
        }

        [Route(Routes.Logout)]
        [HttpPost]
        [UserTypesAllowed(UserType.Unassigned)]
        public IActionResult LogoutUser()
        {
            try
            {
                if (!ClientKeyIsValid())
                    return Unauthorized();

                if (!UserIsAuthenticated())
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
                if (!ClientKeyIsValid())
                    return Unauthorized();

                if (!UserIsAuthenticatedAndAuthorized(MethodBase.GetCurrentMethod()))
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
                if (!ClientKeyIsValid())
                    return Unauthorized();

                if (!UserIsAuthenticatedAndAuthorized(MethodBase.GetCurrentMethod()))
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
                if (!ClientKeyIsValid())
                    return Unauthorized();

                if (!UserIsAuthenticatedAndAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                if (AuthenticatedUser.Type == UserType.SystemUser && AuthenticatedUser.Id != id)
                    return Unauthorized();

                var taskResults = _getUserTask.DoTask(id);

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

        [Route(Routes.User)]
        [HttpPut]
        [UserTypesAllowed(UserType.Unassigned)]
        public IActionResult UpdateUser(int id, User user)
        {
            try
            {
                if (!ClientKeyIsValid())
                    return Unauthorized();

                if (!UserIsAuthenticatedAndAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                if (AuthenticatedUser.Type == UserType.SystemUser && AuthenticatedUser.Id != id)
                    return Unauthorized();

                var invalidUserPathResult = InvalidUserPathResult(id);
                if (invalidUserPathResult != null)
                    return invalidUserPathResult;

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
                if (!ClientKeyIsValid())
                    return Unauthorized();

                if (!UserIsAuthenticatedAndAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                if (AuthenticatedUser.Type == UserType.SystemUser && AuthenticatedUser.Id != userId)
                    return Unauthorized();

                var invalidUserPathResult = InvalidUserPathResult(userId);
                if (invalidUserPathResult != null)
                    return invalidUserPathResult;

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
                if (!ClientKeyIsValid())
                    return Unauthorized();

                if (!UserIsAuthenticatedAndAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                if (AuthenticatedUser.Type == UserType.SystemUser && AuthenticatedUser.Id != userId)
                    return Unauthorized();

                var invalidUserPathResult = InvalidUserPathResult(userId);
                if (invalidUserPathResult != null)
                    return invalidUserPathResult;

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
                if (!ClientKeyIsValid())
                    return Unauthorized();

                if (!UserIsAuthenticatedAndAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                if (AuthenticatedUser.Type == UserType.SystemUser && AuthenticatedUser.Id != userId)
                    return Unauthorized();

                var invalidUserPathResult = InvalidUserPathResult(userId);
                if (invalidUserPathResult != null)
                    return invalidUserPathResult;

                var getUserAccountResult = _getUserAccountTask.DoTask(userAccountId);

                if (getUserAccountResult.HasException)
                    return Error(getUserAccountResult.Exception);

                if (getUserAccountResult.HasNoData)
                    return NotFound();

                if (getUserAccountResult.Data.UserId != userId)
                    return BadRequest();

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
                if (!ClientKeyIsValid())
                    return Unauthorized();

                if (!UserIsAuthenticatedAndAuthorized(MethodBase.GetCurrentMethod()))
                    return Unauthorized();

                if (AuthenticatedUser.Type == UserType.SystemUser && AuthenticatedUser.Id != userId)
                    return Unauthorized();

                var invalidUserPathResult = InvalidUserPathResult(userId);
                if (invalidUserPathResult != null)
                    return invalidUserPathResult;

                var getUserTaskResult = _getUserAccountTask.DoTask(userAccountId);

                if (getUserTaskResult.HasException)
                    return Error(getUserTaskResult.Exception);

                if (getUserTaskResult.HasNoData)
                    return NotFound();

                if (getUserTaskResult.Data.UserId != userId)
                    return BadRequest();

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

        #region Private

        private IActionResult InvalidUserPathResult(int userId)
        {
            var getUserResult = _getUserTask.DoTask(userId);

            if (getUserResult.HasException)
                return Error(getUserResult.Exception);

            if (getUserResult.HasNoData)
                return NotFound();

            return null;
        }

        #endregion
    }
}
