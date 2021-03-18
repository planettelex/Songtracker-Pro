using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.ArtistTasks
{
    public interface IRemoveArtistAccountTask : ITask<ArtistAccount, bool> { }

    public class RemoveArtistAccount : IRemoveArtistAccountTask
    {
        public RemoveArtistAccount(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<bool> DoTask(ArtistAccount artistAccount)
        {
            try
            {
                var toRemove = _dbContext.ArtistAccounts.SingleOrDefault(aa => aa.Id == artistAccount.Id);
                if (toRemove == null)
                    return new TaskResult<bool>(false);

                _dbContext.ArtistAccounts.Remove(toRemove);
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
