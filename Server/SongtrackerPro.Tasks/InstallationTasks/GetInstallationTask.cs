using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.InstallationTasks
{
    public interface IGetInstallationTask : ITask<Nothing, Installation> { }

    public class GetInstallationTask : IGetInstallationTask
    {
        public GetInstallationTask(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<Installation> DoTask(Nothing nothing)
        {
            try
            {
                var installation = _dbContext.Installation.Single();
                return new TaskResult<Installation>(installation);
            }
            catch (Exception e)
            {
                return new TaskResult<Installation>(new TaskException(e));
            }
        }
    }
}