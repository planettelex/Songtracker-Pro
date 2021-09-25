using System;
using System.Collections.Generic;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.RecordLabelTasks
{
    public interface IListReleaseMediaTask : ITask<Release, List<ReleaseMerchandiseProduct>> { }

    public class ListReleaseMedia : TaskBase, IListReleaseMediaTask
    {
        public ListReleaseMedia(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<List<ReleaseMerchandiseProduct>> DoTask(Release release)
        {
            try
            {
                var releaseMedia = _dbContext.ReleaseMedia.Where(rm => rm.ReleaseId == release.Id)
                    .OrderBy(rm => rm.Type)
                    .ToList();

                return new TaskResult<List<ReleaseMerchandiseProduct>>(releaseMedia);
            }
            catch (Exception e)
            {
                return new TaskResult<List<ReleaseMerchandiseProduct>>(new TaskException(e));
            }
        }
    }
}
