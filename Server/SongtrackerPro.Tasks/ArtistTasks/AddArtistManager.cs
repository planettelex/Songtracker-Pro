using System;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.ArtistTasks
{
    public interface IAddArtistManagerTask : ITask<ArtistManager, int?> { }

    public class AddArtistManager : TaskBase, IAddArtistManagerTask
    {
        public AddArtistManager(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<int?> DoTask(ArtistManager artistManager)
        {
            try
            {
                var artistId = artistManager.Artist?.Id ?? artistManager.ArtistId;
                var personId = artistManager.Manager?.Id ?? artistManager.PersonId;
                var hasEnded = artistManager.EndedOn.HasValue && artistManager.EndedOn.Value <= DateTime.Today;

                artistManager.Artist = null;
                artistManager.ArtistId = artistId;
                artistManager.Manager = null;
                artistManager.PersonId = personId;
                artistManager.IsActive = !hasEnded;

                _dbContext.ArtistManagers.Add(artistManager);
                _dbContext.SaveChanges();

                return new TaskResult<int?>(artistManager.Id);
            }
            catch (Exception e)
            {
                return new TaskResult<int?>(new TaskException(e));
            }
        }
    }
}
