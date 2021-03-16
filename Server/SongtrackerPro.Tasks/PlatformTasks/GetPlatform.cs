using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.PlatformTasks
{
    public interface IGetPlatformTask : ITask<int, Platform> { }

    public class GetPlatform : IGetPlatformTask
    {
        public GetPlatform(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<Platform> DoTask(int platformId)
        {
            try
            {
                var platform = _dbContext.Platforms.Where(p => p.Id == platformId)
                    .Include(p => p.PlatformServices).ThenInclude(ps => ps.Service)
                    .SingleOrDefault();

                return new TaskResult<Platform>(platform);
            }
            catch (Exception e)
            {
                return new TaskResult<Platform>(new TaskException(e));
            }
        }
    }
}
