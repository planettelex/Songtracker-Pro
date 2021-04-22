using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.UserTasks
{
    public interface IUpdateUserAuthenticationTask : ITask<User, Nothing> { }

    public class UpdateUserAuthentication : TaskBase, IUpdateUserAuthenticationTask
    {
        public UpdateUserAuthentication(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<Nothing> DoTask(User update)
        {
            try
            {
                var user = _dbContext.Users.SingleOrDefault(u => u.Id == update.Id);

                if (user == null)
                    throw new TaskException(SystemMessage("USER_NOT_FOUND"));

                user.AuthenticationId = update.AuthenticationId;
                user.AuthenticationToken = update.AuthenticationToken;
                _dbContext.SaveChanges();

                return new TaskResult<Nothing>(null as Nothing);
            }
            catch (Exception e)
            {
                return new TaskResult<Nothing>(new TaskException(e));
            }
        }
    }
}
