using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.PlatformTasks
{
    public interface IListPlatformsTask : ITask<Nothing, List<Platform>> { }

    public class ListPlatforms : TaskBase, IListPlatformsTask
    {
        public ListPlatforms(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<List<Platform>> DoTask(Nothing nothing)
        {
            try
            {
                var platforms = _dbContext.Platforms
                    .Include(p => p.PlatformServices).ThenInclude(ps => ps.Service)
                    .ToList();

                return new TaskResult<List<Platform>>(platforms);
            }
            catch (Exception e)
            {
                return new TaskResult<List<Platform>>(new TaskException(e));
            }
        }
    }
}
