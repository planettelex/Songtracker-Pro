using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.ArtistTasks
{
    public interface IGetArtistAccountTask : ITask<int, ArtistAccount> { }

    public class GetArtistAccount : IGetArtistAccountTask
    {
        public GetArtistAccount(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<ArtistAccount> DoTask(int artistAccountId)
        {
            try
            {
                var artistAccount = _dbContext.ArtistAccounts.SingleOrDefault(aa => aa.Id == artistAccountId);

                return new TaskResult<ArtistAccount>(artistAccount);
            }
            catch (Exception e)
            {
                return new TaskResult<ArtistAccount>(new TaskException(e));
            }
        }
    }
}
