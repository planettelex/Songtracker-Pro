using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.RecordLabelTasks
{
    public interface IUpdateReleaseTrackTask : ITask<ReleaseTrack, Nothing> { }

    public class UpdateReleaseTrack : TaskBase, IUpdateReleaseTrackTask
    {
        public UpdateReleaseTrack(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<Nothing> DoTask(ReleaseTrack update)
        {
            try
            {
                var track = _dbContext.ReleaseTracks.SingleOrDefault(rt => rt.Id == update.Id);

                if (track == null)
                    throw new TaskException(SystemMessage("RELEASE_TRACK_NOT_FOUND"));

                track.RecordingId = update.Recording?.Id ?? update.RecordingId;
                track.Recording = _dbContext.Recordings.Single(r => r.Id == track.RecordingId);
                track.TrackNumber = update.TrackNumber;

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
