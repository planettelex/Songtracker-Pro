using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.RecordLabelTasks
{
    public interface IRemoveRecordingTask : ITask<Recording, bool> { }

    public class RemoveRecording : TaskBase, IRemoveRecordingTask
    {
        public RemoveRecording(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<bool> DoTask(Recording recording)
        {
            try
            {
                var toRemove = _dbContext.Recordings.SingleOrDefault(r => r.Id == recording.Id);
                if (toRemove == null)
                    return new TaskResult<bool>(false);

                _dbContext.Recordings.Remove(toRemove);
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
