using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.InstallationTasks
{
    public interface IGetInstallationTask { TaskResult<Installation> DoTask(); }

    public class GetInstallationTask : ITask<Installation>, IGetInstallationTask
    {
        public GetInstallationTask(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<Installation> DoTask()
        {
            try
            {
                var installation = _dbContext.Installation.Single();
                return new TaskResult<Installation>(true, installation);
            }
            catch (Exception e)
            {
                return new TaskResult<Installation>(new TaskException(e));
            }
        }
    }
}