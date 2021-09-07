using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.ArtistTasks
{
    public interface IUpdateArtistAccountTask : ITask<ArtistAccount, Nothing> { }

    public class UpdateArtistAccount : TaskBase, IUpdateArtistAccountTask
    {
        public UpdateArtistAccount(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<Nothing> DoTask(ArtistAccount update)
        {
            try
            {
                var artistAccount = _dbContext.ArtistAccounts.SingleOrDefault(aa => aa.Id == update.Id);

                if (artistAccount == null)
                    throw new TaskException(SystemMessage("ARTIST_ACCOUNT_NOT_FOUND"));

                if (update.IsPreferred)
                {
                    var allArtistAccounts = _dbContext.ArtistAccounts.ToList();
                    foreach (var account in allArtistAccounts)
                        account.IsPreferred = false;

                    _dbContext.SaveChanges();
                }

                artistAccount.PlatformId = update.Platform?.Id ?? update.PlatformId;
                artistAccount.Platform = _dbContext.Platforms.Single(p => p.Id == artistAccount.PlatformId);
                artistAccount.IsPreferred = update.IsPreferred;
                artistAccount.Username = update.Username;
                _dbContext.SaveChanges();

                return new TaskResult<Nothing>(true);
            }
            catch (Exception e)
            {
                return new TaskResult<Nothing>(new TaskException(e));
            }
        }
    }
}
