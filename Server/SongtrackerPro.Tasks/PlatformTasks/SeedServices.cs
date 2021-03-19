using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.PlatformTasks
{
    public interface ISeedServicesTask : ITask<Nothing, bool> { }

    public class SeedServices : TaskBase, ISeedServicesTask
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

                _dbContext.Services.Add(new Service { Name = SeedData("SERVICE_INTERACTIVE_STREAMING") });
                _dbContext.Services.Add(new Service { Name = SeedData("SERVICE_NON_INTERACTIVE_STREAMING") });
                _dbContext.Services.Add(new Service { Name = SeedData("SERVICE_LIVE_STREAMING") });
                _dbContext.Services.Add(new Service { Name = SeedData("SERVICE_VIDEO_HOSTING") });
                _dbContext.Services.Add(new Service { Name = SeedData("SERVICE_VIDEO_CLIPS") });
                _dbContext.Services.Add(new Service { Name = SeedData("SERVICE_SOCIAL_MEDIA") });
                _dbContext.Services.Add(new Service { Name = SeedData("SERVICE_DIGITAL_SALES") });
                _dbContext.Services.Add(new Service { Name = SeedData("SERVICE_PHYSICAL_SALES") });
                _dbContext.Services.Add(new Service { Name = SeedData("SERVICE_PAYMENT") });
                _dbContext.Services.Add(new Service { Name = SeedData("SERVICE_MUSIC_IDENTIFICATION") });
                _dbContext.Services.Add(new Service { Name = SeedData("SERVICE_EVENT_MANAGEMENT") });

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