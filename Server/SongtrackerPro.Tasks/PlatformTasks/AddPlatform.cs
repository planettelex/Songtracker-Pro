using System;
using System.Collections.Generic;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.PlatformTasks
{
    public interface IAddPlatformTask : ITask<Platform, int?> { }

    public class AddPlatform : TaskBase, IAddPlatformTask
    {
        public AddPlatform(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<int?> DoTask(Platform platform)
        {
            try
            {
                _dbContext.Platforms.Add(platform);
                _dbContext.SaveChanges();

                if (platform.Services == null || !platform.Services.Any()) 
                    return new TaskResult<int?>(platform.Id);

                platform.PlatformServices = new List<PlatformService>();
                foreach (var service in platform.Services)
                {
                    platform.PlatformServices.Add(new PlatformService
                    {
                        PlatformId = platform.Id, 
                        ServiceId = service.Id
                    });
                }
                _dbContext.SaveChanges();

                return new TaskResult<int?>(platform.Id);
            }
            catch (Exception e)
            {
                return new TaskResult<int?>(new TaskException(e));
            }
        }
    }
}
