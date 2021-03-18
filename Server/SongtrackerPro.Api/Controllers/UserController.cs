using Microsoft.AspNetCore.Mvc;
using SongtrackerPro.Data.Enums;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Tasks.UserTasks;

namespace SongtrackerPro.Api.Controllers
{
    [ApiController]
    public class UserController : ApiControllerBase
    {
        public UserController(IListUsersTask listUsersTask,
                              IGetUserTask getUserTask,
                              IAddUserTask addUserTask,
                              IUpdateUserTask updateUserTask,
                              ILoginUserTask loginUserTask,
                              IListUserAccountsTask listUserAccountsTask,
                              IGetUserAccountTask getUserAccountTask,
                              IAddUserAccountTask addUserAccountTask,
                              IUpdateUserAccountTask updateUserAccountTask,
                              IRemoveUserAccountTask removeUserAccountTask)
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

        [Route(Routes.Users)]
        [HttpPost]
        public string AddUser(User user)
        {
            var taskResults = _addUserTask.DoTask(user);

            return JsonSerialize(taskResults);
        }

        [Route(Routes.Users)]
        [HttpGet]
        public string ListUsers(UserType? type)
        {
            var taskResults = _listUsersTask.DoTask(type);

            return JsonSerialize(taskResults);
        }

        [Route(Routes.User)]
        [HttpGet]
        public string GetUser(int id)
        {
            var taskResults = _getUserTask.DoTask(id);

            return JsonSerialize(taskResults);
        }

        [Route(Routes.User)]
        [HttpPut]
        public void UpdateUser(int id, User user)
        {
            user.Id = id;
            _updateUserTask.DoTask(user);
        }

        [Route(Routes.Login)]
        [HttpPost]
        public string LoginUser(Login login)
        {
            var taskResults = _loginUserTask.DoTask(login);

            return JsonSerialize(taskResults);
        }

        [Route(Routes.UserAccounts)]
        [HttpPost]
        public string AddUserAccount(int userId, UserAccount userAccount)
        {
            userAccount.UserId = userId;
            var taskResults = _addUserAccountTask.DoTask(userAccount);

            return JsonSerialize(taskResults);
        }

        [Route(Routes.UserAccounts)]
        [HttpGet]
        public string ListUserAccounts(int userId)
        {
            var user = _getUserTask.DoTask(userId).Data;
            var taskResults = _listUserAccountsTask.DoTask(user);

            return JsonSerialize(taskResults);
        }

        [Route(Routes.UserAccount)]
        [HttpPut]
        public void UpdateUserAccount(int userId, int userAccountId, UserAccount userAccount)
        {
            userAccount.UserId = userId;
            userAccount.Id = userAccountId;
            _updateUserAccountTask.DoTask(userAccount);
        }

        [Route(Routes.UserAccount)]
        [HttpDelete]
        public void RemoveUserAccount(int userId, int userAccountId)
        {
            var toRemove = _getUserAccountTask.DoTask(userAccountId).Data;
            if (toRemove == null)
                return;

            if (toRemove.UserId == userId)
                _removeUserAccountTask.DoTask(toRemove);
        }
    }
}
