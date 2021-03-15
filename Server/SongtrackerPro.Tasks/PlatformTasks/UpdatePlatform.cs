using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.PlatformTasks
{
    public interface IUpdatePlatformTask : ITask<Platform, Nothing> { }

    public class UpdatePlatform : IUpdatePlatformTask
    {
        public UpdatePlatform(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<Nothing> DoTask(Platform update)
        {
            try
            {
                var platform = _dbContext.Platforms.Where(p => p.Id == update.Id).Include(p => p.PlatformServices).ThenInclude(ps => ps.Service).SingleOrDefault();
                if (platform == null)
                    throw new TaskException("Platform not found.");

                platform.Name = update.Name;
                platform.Website = update.Website;
                _dbContext.SaveChanges();

                if (update.Services != null)
                {
                    foreach (var platformService in platform.PlatformServices)
                    {
                        var service = update.Services.SingleOrDefault(s => s.Id == platformService.ServiceId);
                        if (service == null)
                            _dbContext.PlatformServices.Remove(platformService);
                    }
                    _dbContext.SaveChanges();

                    foreach (var service in update.Services)
                    {
                        var platformService = platform.PlatformServices.SingleOrDefault(ps => ps.ServiceId == service.Id);
                        if (platformService == null)
                            platform.PlatformServices.Add(new PlatformService { PlatformId = platform.Id, ServiceId = service.Id });
                    }
                    _dbContext.SaveChanges();
                }

                return new TaskResult<Nothing>(null as Nothing);
            }
            catch (Exception e)
            {
                return new TaskResult<Nothing>(new TaskException(e));
            }
        }
    }
}
