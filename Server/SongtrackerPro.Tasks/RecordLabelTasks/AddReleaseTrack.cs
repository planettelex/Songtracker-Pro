using System;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.RecordLabelTasks
{
    public interface IAddReleaseTrackTask : ITask<ReleaseTrack, int?> { }

    public class AddReleaseTrack : TaskBase, IAddReleaseTrackTask
    {
        public AddReleaseTrack(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<int?> DoTask(ReleaseTrack releaseTrack)
        {
            try
            {
                var releaseId = releaseTrack.Release?.Id ?? releaseTrack.ReleaseId;
                var recordingId = releaseTrack.Recording?.Id ?? releaseTrack.RecordingId;

                releaseTrack.Release = null;
                releaseTrack.ReleaseId = releaseId;
                releaseTrack.Recording = null;
                releaseTrack.RecordingId = recordingId;

                _dbContext.ReleaseTracks.Add(releaseTrack);
                _dbContext.SaveChanges();

                return new TaskResult<int?>(releaseTrack.Id);
            }
            catch (Exception e)
            {
                return new TaskResult<int?>(new TaskException(e));
            }
        }
    }
}
