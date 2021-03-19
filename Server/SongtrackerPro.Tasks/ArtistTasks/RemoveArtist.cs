using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.ArtistTasks
{
    public interface IRemoveArtistTask : ITask<Artist, bool> { }

    public class RemoveArtist : TaskBase, IRemoveArtistTask
    {
        public RemoveArtist(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<bool> DoTask(Artist artist)
        {
            try
            {
                var toRemove = _dbContext.Artists.SingleOrDefault(a => a.Id == artist.Id);
                if (toRemove == null)
                    return new TaskResult<bool>(false);

                _dbContext.Artists.Remove(toRemove);
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
