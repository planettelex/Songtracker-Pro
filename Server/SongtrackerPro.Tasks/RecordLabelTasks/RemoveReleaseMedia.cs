using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.RecordLabelTasks
{
    public interface IRemoveReleaseMediaTask : ITask<ReleaseMerchandiseProduct, bool> { }

    public class RemoveReleaseMedia : TaskBase, IRemoveReleaseMediaTask
    {
        public RemoveReleaseMedia(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<bool> DoTask(ReleaseMerchandiseProduct releaseMedia)
        {
            try
            {
                var toRemove = _dbContext.ReleaseMedia.SingleOrDefault(rm => rm.Id == releaseMedia.Id);
                if (toRemove == null)
                    return new TaskResult<bool>(false);

                _dbContext.ReleaseMedia.Remove(toRemove);
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
