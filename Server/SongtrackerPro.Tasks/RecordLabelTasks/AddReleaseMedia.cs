using System;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.RecordLabelTasks
{
    public interface IAddReleaseMediaTask : ITask<ReleaseMerchandiseProduct, int?> { }

    public class AddReleaseMedia : TaskBase, IAddReleaseMediaTask
    {
        public AddReleaseMedia(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<int?> DoTask(ReleaseMerchandiseProduct releaseMedia)
        {
            try
            {
                var releaseId = releaseMedia.Release?.Id ?? releaseMedia.ReleaseId;

                releaseMedia.Release = null;
                releaseMedia.ReleaseId = releaseId;

                _dbContext.ReleaseMedia.Add(releaseMedia);
                _dbContext.SaveChanges();

                return new TaskResult<int?>(releaseMedia.Id);
            }
            catch (Exception e)
            {
                return new TaskResult<int?>(new TaskException(e));
            }
        }
    }
}
