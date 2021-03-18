using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.ArtistTasks
{
    public interface IRemoveArtistLinkTask : ITask<ArtistLink, bool> { }

    public class RemoveArtistLink : IRemoveArtistLinkTask
    {
        public RemoveArtistLink(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<bool> DoTask(ArtistLink artistLink)
        {
            try
            {
                var toRemove = _dbContext.ArtistLinks.SingleOrDefault(al => al.Id == artistLink.Id);
                if (toRemove == null)
                    return new TaskResult<bool>(false);

                _dbContext.ArtistLinks.Remove(toRemove);
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
