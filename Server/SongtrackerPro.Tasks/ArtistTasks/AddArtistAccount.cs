using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.ArtistTasks
{
    public interface IAddArtistAccountTask : ITask<ArtistAccount, int?> { }

    public class AddArtistAccount : TaskBase, IAddArtistAccountTask
    {
        public AddArtistAccount(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<int?> DoTask(ArtistAccount artistAccount)
        {
            try
            {
                if (artistAccount.IsPreferred)
                {
                    var allArtistAccounts = _dbContext.ArtistAccounts.ToList();
                    foreach (var account in allArtistAccounts)
                        account.IsPreferred = false;

                    _dbContext.SaveChanges();
                }

                var artistId = artistAccount.Artist?.Id ?? artistAccount.ArtistId;
                var platformId = artistAccount.Platform?.Id ?? artistAccount.PlatformId;

                artistAccount.Artist = null;
                artistAccount.ArtistId = artistId;
                artistAccount.Platform = null;
                artistAccount.PlatformId = platformId;

                _dbContext.ArtistAccounts.Add(artistAccount);
                _dbContext.SaveChanges();

                return new TaskResult<int?>(artistAccount.Id);
            }
            catch (Exception e)
            {
                return new TaskResult<int?>(new TaskException(e));
            }
        }
    }
}
