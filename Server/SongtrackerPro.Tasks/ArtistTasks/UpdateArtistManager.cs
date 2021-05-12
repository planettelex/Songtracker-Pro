using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.ArtistTasks
{
    public interface IUpdateArtistManagerTask : ITask<ArtistManager, Nothing> { }

    public class UpdateArtistManager : TaskBase, IUpdateArtistManagerTask
    {
        public UpdateArtistManager(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<Nothing> DoTask(ArtistManager update)
        {
            try
            {
                var artistManager = _dbContext.ArtistManagers.SingleOrDefault(am => am.Id == update.Id);
                if (artistManager == null)
                    throw new TaskException(SystemMessage("ARTIST_MANAGER_NOT_FOUND"));

                var hasEnded = update.EndedOn.HasValue && update.EndedOn.Value <= DateTime.Today;
                artistManager.StartedOn = update.StartedOn;
                artistManager.EndedOn = update.EndedOn;
                artistManager.IsActive = !hasEnded;
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
