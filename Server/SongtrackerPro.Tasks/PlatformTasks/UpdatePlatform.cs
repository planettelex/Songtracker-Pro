﻿using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.PlatformTasks
{
    public interface IUpdatePlatformTask : ITask<Platform, Nothing> { }

    public class UpdatePlatform : TaskBase, IUpdatePlatformTask
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
                var platform = _dbContext.Platforms.Where(p => p.Id == update.Id)
                    .Include(p => p.PlatformServices).ThenInclude(ps => ps.Service).SingleOrDefault();

                if (platform == null)
                    throw new TaskException(SystemMessage("PLATFORM_NOT_FOUND"));

                platform.Name = update.Name;
                platform.Website = string.IsNullOrWhiteSpace(update.Website) ? null : update.Website;
                _dbContext.SaveChanges();

                if (update.Services == null) 
                    return new TaskResult<Nothing>(null as Nothing);
                
                foreach (var platformService in platform.PlatformServices)
                {
                    var service = update.Services.SingleOrDefault(s => s?.Id == platformService.ServiceId);
                    if (service == null)
                        _dbContext.PlatformServices.Remove(platformService);
                }
                _dbContext.SaveChanges();

                foreach (var service in update.Services.Where(service => service != null))
                {
                    var platformService = platform.PlatformServices.SingleOrDefault(ps => ps.ServiceId == service.Id);
                    if (platformService == null)
                        platform.PlatformServices.Add(new PlatformService { PlatformId = platform.Id, ServiceId = service.Id });
                }
                _dbContext.SaveChanges();

                return new TaskResult<Nothing>(true);
            }
            catch (Exception e)
            {
                return new TaskResult<Nothing>(new TaskException(e));
            }
        }
    }
}
