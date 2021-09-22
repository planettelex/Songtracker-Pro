using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.RecordLabelTasks
{
    public interface IRemoveReleaseTrackTask : ITask<ReleaseTrack, bool> { }

    public class RemoveReleaseTrack : TaskBase, IRemoveReleaseTrackTask
    {
        public RemoveReleaseTrack(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<bool> DoTask(ReleaseTrack releaseTrack)
        {
            try
            {
                var toRemove = _dbContext.ReleaseTracks.SingleOrDefault(rt => rt.Id == releaseTrack.Id);
                if (toRemove == null)
                    return new TaskResult<bool>(false);

                _dbContext.ReleaseTracks.Remove(toRemove);
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
