using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Utilities;

namespace SongtrackerPro.Tasks.InstallationTasks
{
    public interface ISeedInstallationTask : ITask<Nothing, bool> { }

    public class SeedInstallation : ISeedInstallationTask
    {
        public SeedInstallation(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<bool> DoTask(Nothing input)
        {
            try
            {
                var installation = _dbContext.Installation.SingleOrDefault();
                if (installation != null)
                    return new TaskResult<bool>(false);

                installation = new Installation
                {
                    Uuid = Guid.NewGuid(),
                    Version = ApplicationSettings.Version,
                    Name = "Songtracker Pro",
                    Tagline = "Royalties Tracking and Management"
                };

                _dbContext.Installation.Add(installation);
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
