using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.UserTasks
{
    public interface IUpdateUserAccountTask : ITask<UserAccount, Nothing> { }

    public class UpdateUserAccount : TaskBase, IUpdateUserAccountTask
    {
        public UpdateUserAccount(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<Nothing> DoTask(UserAccount update)
        {
            try
            {
                var userAccount = _dbContext.UserAccounts.SingleOrDefault(ua => ua.Id == update.Id);

                if (userAccount == null)
                    throw new TaskException(SystemMessage("USER_ACCOUNT_NOT_FOUND"));

                if (update.IsPreferred)
                {
                    var allUserAccounts = _dbContext.UserAccounts.ToList();
                    foreach (var account in allUserAccounts)
                        account.IsPreferred = false;

                    _dbContext.SaveChanges();
                }

                userAccount.IsPreferred = update.IsPreferred;
                userAccount.Username = update.Username;
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
