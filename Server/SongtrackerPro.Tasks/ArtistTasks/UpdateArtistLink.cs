using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.ArtistTasks
{
    public interface IUpdateArtistLinkTask : ITask<ArtistLink, Nothing> { }

    public class UpdateArtistLink : TaskBase, IUpdateArtistLinkTask
    {
        public UpdateArtistLink(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<Nothing> DoTask(ArtistLink update)
        {
            try
            {
                var artistLink = _dbContext.ArtistLinks.SingleOrDefault(al => al.Id == update.Id);

                if (artistLink == null)
                    throw new TaskException(SystemMessage("ARTIST_ACCOUNT_NOT_FOUND"));

                artistLink.PlatformId = update.Platform?.Id ?? update.PlatformId;
                artistLink.Platform = _dbContext.Platforms.Single(p => p.Id == artistLink.PlatformId);
                artistLink.Url = update.Url;
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
