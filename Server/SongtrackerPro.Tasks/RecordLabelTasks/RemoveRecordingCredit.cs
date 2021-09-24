using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.RecordLabelTasks
{
    public interface IRemoveRecordingCreditTask : ITask<RecordingCredit, bool> { }

    public class RemoveRecordingCredit : TaskBase, IRemoveRecordingCreditTask
    {
        public RemoveRecordingCredit(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<bool> DoTask(RecordingCredit recordingCredit)
        {
            try
            {
                var toRemove = _dbContext.RecordingCredits.SingleOrDefault(rc => rc.Id == recordingCredit.Id);
                if (toRemove == null)
                    return new TaskResult<bool>(false);

                _dbContext.RecordingCredits.Remove(toRemove);
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
