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
                              ILoginUserTask loginUserTask)
        {
            _listUsersTask = listUsersTask;
            _getUserTask = getUserTask;
            _addUserTask = addUserTask;
            _updateUserTask = updateUserTask;
            _loginUserTask = loginUserTask;
        }
        private readonly IListUsersTask _listUsersTask;
        private readonly IGetUserTask _getUserTask;
        private readonly IAddUserTask _addUserTask;
        private readonly IUpdateUserTask _updateUserTask;
        private readonly ILoginUserTask _loginUserTask;

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
    }
}
