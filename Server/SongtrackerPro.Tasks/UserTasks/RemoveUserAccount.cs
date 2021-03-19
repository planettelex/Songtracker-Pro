using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.UserTasks
{
    public interface IRemoveUserAccountTask : ITask<UserAccount, bool> { }

    public class RemoveUserAccount : TaskBase, IRemoveUserAccountTask
    {
        public RemoveUserAccount(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<bool> DoTask(UserAccount userAccount)
        {
            try
            {
                var toRemove = _dbContext.UserAccounts.SingleOrDefault(ua => ua.Id == userAccount.Id);
                if (toRemove == null)
                    return new TaskResult<bool>(false);

                _dbContext.UserAccounts.Remove(toRemove);
                _dbContext.SaveChanges();

                return new TaskResult<bool>(true);
            }
            catch (Exception e)
            {
                return new TaskResult<bool>(new TaskException(e));
            }
        }
    }
}
