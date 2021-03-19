using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.PlatformTasks
{
    public interface IRemovePlatformTask : ITask<Platform, bool> { }

    public class RemovePlatform : TaskBase, IRemovePlatformTask
    {
        public RemovePlatform(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<bool> DoTask(Platform platform)
        {
            try
            {
                var toRemove = _dbContext.Platforms.SingleOrDefault(p => p.Id == platform.Id);
                if (toRemove == null)
                    return new TaskResult<bool>(false);

                _dbContext.Platforms.Remove(toRemove);
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
