using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.PlatformTasks
{
    public interface ISeedServicesTask : ITask<Nothing, bool> { }

    public class SeedServices : ISeedServicesTask
    {
        public SeedServices(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<bool> DoTask(Nothing nothing)
        {
            try
            {
                var services = _dbContext.Services.ToList();
                if (services.Any())
                    return new TaskResult<bool>(false);

                _dbContext.Services.Add(new Service { Name = "Interactive Streaming" });
                _dbContext.Services.Add(new Service { Name = "Non-Interactive Streaming" });
                _dbContext.Services.Add(new Service { Name = "Live Streaming" });
                _dbContext.Services.Add(new Service { Name = "Video Hosting" });
                _dbContext.Services.Add(new Service { Name = "Video Clips" });
                _dbContext.Services.Add(new Service { Name = "Social Media" });
                _dbContext.Services.Add(new Service { Name = "Digital Sales" });
                _dbContext.Services.Add(new Service { Name = "Physical Sales" });
                _dbContext.Services.Add(new Service { Name = "Payment" });

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