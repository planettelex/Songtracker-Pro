using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.RecordLabelTasks
{
    public interface IAddReleaseTask : ITask<Release, int?> { }

    public class AddRelease  : TaskBase, IAddReleaseTask
    {
        public AddRelease(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<int?> DoTask(Release release)
        {
            try
            {
                var artistId = release.Artist?.Id ?? release.ArtistId;
                var artist = _dbContext.Artists
                    .Where(a => a.Id == artistId)
                    .Include(a => a.RecordLabel).ThenInclude(rl => rl.Address).ThenInclude(a => a.Country)
                    .SingleOrDefault();

                var genreId = release.Genre?.Id ?? release.GenreId;
                var genre = _dbContext.Genres
                    .Where(g => g.Id == genreId)
                    .Include(g => g.ParentGenre).ThenInclude(pg => pg.ParentGenre)
                    .SingleOrDefault();

                var recordLabelId = release.RecordLabel?.Id ?? release.RecordLabelId;
                var recordLabel = _dbContext.RecordLabels
                    .Where(rl => rl.Id == recordLabelId)
                    .Include(rl => rl.Address).ThenInclude(a => a.Country)
                    .Single();

                release.Artist = null;
                release.ArtistId = artistId;
                release.Genre = null;
                release.GenreId = genreId;
                release.RecordLabel = null;
                release.RecordLabelId = recordLabelId;

                _dbContext.Releases.Add(release);
                _dbContext.SaveChanges();

                release.Artist = artist;
                release.Genre = genre;
                release.RecordLabel = recordLabel;

                return new TaskResult<int?>(release.Id);
            }
            catch (Exception e)
            {
                return new TaskResult<int?>(new TaskException(e));
            }
        }
    }
}
