using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Enums;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Utilities;

namespace SongtrackerPro.Tasks.UserTasks
{
    public interface ISeedSuperuserTask : ITask<Nothing, bool> { }

    public class SeedSuperuser : TaskBase, ISeedSuperuserTask
    {
        public SeedSuperuser(ApplicationDbContext dbContext, IAddUserTask addUserTask)
        {
            _dbContext = dbContext;
            _addUserTask = addUserTask;
        }
        private readonly ApplicationDbContext _dbContext;
        private readonly IAddUserTask _addUserTask;

        public TaskResult<bool> DoTask(Nothing nothing)
        {
            try
            {
                var superuser = _dbContext.Users.SingleOrDefault(u => u.AuthenticationId == ApplicationSettings.Mail.From);
                if (superuser != null)
                    return new TaskResult<bool>(false);

                superuser = new User
                {
                    AuthenticationId = ApplicationSettings.Mail.From,
                    Type = UserType.SystemAdministrator
                };
                _addUserTask.DoTask(superuser);

                return new TaskResult<bool>(true);
            }
            catch (Exception e)
            {
                return new TaskResult<bool>(new TaskException(e));
            }
        }
    }
}
