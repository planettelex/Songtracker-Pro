using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.RecordLabelTasks
{
    public interface IAddRecordingTask : ITask<Recording, int?> { }

    public class AddRecording : TaskBase, IAddRecordingTask
    {
        public AddRecording(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<int?> DoTask(Recording recording)
        {
            try
            {
                var artistId = recording.Artist?.Id ?? recording.ArtistId;
                var recordLabelId = recording.Artist?.Id ?? recording.RecordLabelId;
                var compositionId = recording.Composition?.Id ?? recording.CompositionId;
                var originalRecordingId = recording.OriginalRecording?.Id ?? recording.OriginalRecordingId;

                var artist = _dbContext.Artists.Where(a => a.Id == artistId)
                    .Include(a => a.Address).ThenInclude(a => a.Country)
                    .Include(a => a.RecordLabel).ThenInclude(rl => rl.Address).ThenInclude(a => a.Country)
                    .Single();
                var recordLabel = _dbContext.RecordLabels.Where(rl => rl.Id == recordLabelId)
                    .Include(rl => rl.Address).ThenInclude(a => a.Country)
                    .Single();
                var composition = _dbContext.Compositions.Where(c => c.Id == compositionId)
                    .Include(c => c.Publisher).ThenInclude(p => p.Address).ThenInclude(a => a.Country)
                    .Include(c => c.Publisher).ThenInclude(p => p.PerformingRightsOrganization).ThenInclude(o => o.Country)
                    .Include(c => c.ExternalPublisher).ThenInclude(ep => ep.Address).ThenInclude(a => a.Country)
                    .Single();
                var originalRecording = _dbContext.Recordings.Where(or => or.Id == originalRecordingId)
                    .Include(or => or.Artist).ThenInclude(a => a.Address).ThenInclude(a => a.Country)
                    .Include(or => or.Composition).ThenInclude(c => c.Publisher).ThenInclude(p => p.Address).ThenInclude(a => a.Country)
                    .Include(or => or.RecordLabel).ThenInclude(rl => rl.Address).ThenInclude(a => a.Country)
                    .SingleOrDefault();

                recording.Artist = null;
                recording.ArtistId = artistId;
                recording.RecordLabel = null;
                recording.RecordLabelId = recordLabelId;
                recording.Composition = null;
                recording.CompositionId = compositionId;
                recording.OriginalRecording = null;
                recording.OriginalRecordingId = originalRecordingId;

                _dbContext.Recordings.Add(recording);
                _dbContext.SaveChanges();

                recording.Artist = artist;
                recording.RecordLabel = recordLabel;
                recording.Composition = composition;
                recording.OriginalRecording = originalRecording;
                
                return new TaskResult<int?>(recording.Id);
            }
            catch (Exception e)
            {
                return new TaskResult<int?>(new TaskException(e));
            }
        }
    }
}
