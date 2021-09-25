using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.RecordLabelTasks
{
    public interface IGetReleaseMediaTask : ITask<int, ReleaseMerchandiseProduct> { }

    public class GetReleaseMedia : TaskBase, IGetReleaseMediaTask
    {
        public GetReleaseMedia(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<ReleaseMerchandiseProduct> DoTask(int releaseMediaId)
        {
            try
            {
                var releaseMedia = _dbContext.ReleaseMedia.Where(rm => rm.Id == releaseMediaId)
                    .Include(rm => rm.Release).ThenInclude(r => r.RecordLabel).ThenInclude(rl => rl.Address).ThenInclude(a => a.Country)
                    .SingleOrDefault();

                return new TaskResult<ReleaseMerchandiseProduct>(releaseMedia);
            }
            catch (Exception e)
            {
                return new TaskResult<ReleaseMerchandiseProduct>(new TaskException(e));
            }
        }
    }
}
