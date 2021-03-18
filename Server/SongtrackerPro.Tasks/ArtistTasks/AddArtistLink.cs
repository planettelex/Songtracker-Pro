using System;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.ArtistTasks
{
    public interface IAddArtistLinkTask : ITask<ArtistLink, int?> { }

    public class AddArtistLink : IAddArtistLinkTask
    {
        public AddArtistLink(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<int?> DoTask(ArtistLink artistLink)
        {
            try
            {
                var artistId = artistLink.Artist?.Id ?? artistLink.ArtistId;
                var platformId = artistLink.Platform?.Id ?? artistLink.PlatformId;

                artistLink.Artist = null;
                artistLink.ArtistId = artistId;
                artistLink.Platform = null;
                artistLink.PlatformId = platformId;

                _dbContext.ArtistLinks.Add(artistLink);
                _dbContext.SaveChanges();

                return new TaskResult<int?>(artistLink.Id);
            }
            catch (Exception e)
            {
                return new TaskResult<int?>(new TaskException(e));
            }
        }
    }
}
