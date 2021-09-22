using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.RecordLabelTasks
{
    public interface IRemoveReleaseTask : ITask<Release, bool> { }

    public class RemoveRelease : TaskBase, IRemoveReleaseTask
    {
        public RemoveRelease(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<bool> DoTask(Release release)
        {
            try
            {
                var toRemove = _dbContext.Releases.SingleOrDefault(r => r.Id == release.Id);
                if (toRemove == null)
                    return new TaskResult<bool>(false);

                _dbContext.Releases.Remove(toRemove);
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
