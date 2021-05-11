using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.UserTasks
{
    public interface ILogoutUserTask : ITask<User, Nothing> { }

    public class LogoutUser : TaskBase, ILogoutUserTask
    {
        public LogoutUser(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<Nothing> DoTask(User loggedInUser)
        {
            try
            {
                var user = _dbContext.Users.SingleOrDefault(u => u.Id == loggedInUser.Id);

                if (user == null)
                    throw new TaskException(SystemMessage("USER_NOT_FOUND"));

                var lastLogin = _dbContext.Logins.Where(l => l.UserId == user.Id).OrderByDescending(l => l.LoginAt).FirstOrDefault();

                if (lastLogin != null)
                {
                    lastLogin.LogoutAt = DateTime.UtcNow;
                    _dbContext.SaveChanges();
                }
                
                return new TaskResult<Nothing>(null as Nothing);
            }
            catch (Exception e)
            {
                return new TaskResult<Nothing>(new TaskException(e));
            }
        }
    }
}
