using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.UserTasks
{
    public interface IAddUserAccountTask : ITask<UserAccount, int?> { }

    public class AddUserAccount : TaskBase, IAddUserAccountTask
    {
        public AddUserAccount(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<int?> DoTask(UserAccount userAccount)
        {
            try
            {
                if (userAccount.IsPreferred)
                {
                    var allUserAccounts = _dbContext.UserAccounts.ToList();
                    foreach (var account in allUserAccounts)
                        account.IsPreferred = false;

                    _dbContext.SaveChanges();
                }

                var userId = userAccount.User?.Id ?? userAccount.UserId;
                var platformId = userAccount.Platform?.Id ?? userAccount.PlatformId;

                userAccount.User = null;
                userAccount.UserId = userId;
                userAccount.Platform = null;
                userAccount.PlatformId = platformId;

                _dbContext.UserAccounts.Add(userAccount);
                _dbContext.SaveChanges();

                return new TaskResult<int?>(userAccount.Id);
            }
            catch (Exception e)
            {
                return new TaskResult<int?>(new TaskException(e));
            }
        }
    }
}
