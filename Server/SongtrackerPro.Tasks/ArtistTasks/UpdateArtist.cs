using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.ArtistTasks
{
    public interface IUpdateArtistTask : ITask<Artist, Nothing> { }

    public class UpdateArtist : TaskBase, IUpdateArtistTask
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
                    throw new TaskException(SystemMessage("ARTIST_NOT_FOUND"));

                artist.Name = update.Name;
                artist.TaxId = update.TaxId;
                artist.HasServiceMark = update.HasServiceMark;
                artist.WebsiteUrl = update.WebsiteUrl;
                artist.PressKitUrl = update.PressKitUrl;

                artist.RecordLabelId = update.RecordLabel?.Id;
                if (artist.RecordLabelId.HasValue)
                {
                    var recordLabel = _dbContext.RecordLabels.SingleOrDefault(l => l.Id == artist.RecordLabelId);
                    artist.RecordLabel = recordLabel ?? throw new TaskException(SystemMessage("RECORD_LABEL_NOT_FOUND"));
                }

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
