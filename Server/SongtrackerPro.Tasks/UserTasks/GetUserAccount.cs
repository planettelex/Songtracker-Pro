using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.UserTasks
{
    public interface IGetUserAccountTask : ITask<int, UserAccount> { }

    public class GetUserAccount : TaskBase, IGetUserAccountTask
    {
        public GetUserAccount(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<UserAccount> DoTask(int userAccountId)
        {
            try
            {
                var userAccount = _dbContext.UserAccounts.SingleOrDefault(ua => ua.Id == userAccountId);

                return new TaskResult<UserAccount>(userAccount);
            }
            catch (Exception e)
            {
                return new TaskResult<UserAccount>(new TaskException(e));
            }
        }
    }
}
