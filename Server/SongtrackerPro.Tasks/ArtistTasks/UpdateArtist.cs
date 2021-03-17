using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.ArtistTasks
{
    public interface IUpdateArtistTask : ITask<Artist, Nothing> { }

    public class UpdateArtist : IUpdateArtistTask
    {
        public UpdateArtist(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<Nothing> DoTask(Artist update)
        {
            try
            {
                var artist = _dbContext.Artists
                    .SingleOrDefault(a => a.Id == update.Id);

                if (artist == null)
                    throw new TaskException("Artist not found.");

                artist.Name = update.Name;
                artist.TaxId = update.TaxId;
                artist.HasServiceMark = update.HasServiceMark;
                artist.WebsiteUrl = update.WebsiteUrl;
                artist.PressKitUrl = update.PressKitUrl;
                artist.RecordLabel = null;
                artist.RecordLabelId = update.RecordLabel?.Id ?? update.RecordLabelId;
                _dbContext.SaveChanges();

                return new TaskResult<Nothing>(null as Nothing);
            }
            catch (Exception e)
            {
                return new TaskResult<Nothing>(new TaskException(e));
            }
        }
    }
}
