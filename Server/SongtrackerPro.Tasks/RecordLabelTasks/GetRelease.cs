using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.RecordLabelTasks
{
    public interface IGetReleaseTask : ITask<int, Release> { }

    public class GetRelease : TaskBase, IGetReleaseTask
    {
        public GetRelease(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<Release> DoTask(int releaseId)
        {
            try
            {
                var release = _dbContext.Releases.Where(r => r.Id == releaseId)
                    .Include(r => r.RecordLabel).ThenInclude(rl => rl.Address).ThenInclude(a => a.Country)
                    .SingleOrDefault();

                return new TaskResult<Release>(release);

            }
            catch (Exception e)
            {
                return new TaskResult<Release>(new TaskException(e));
            }
        }
    }
}
