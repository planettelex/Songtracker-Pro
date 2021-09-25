using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.RecordLabelTasks
{
    public interface IUpdateReleaseMediaTask : ITask<ReleaseMerchandiseProduct, Nothing> { }

    public class UpdateReleaseMedia : TaskBase, IUpdateReleaseMediaTask
    {
        public UpdateReleaseMedia(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<Nothing> DoTask(ReleaseMerchandiseProduct update)
        {
            try
            {
                var releaseMedia = _dbContext.ReleaseMedia.SingleOrDefault(rm => rm.Id == update.Id);

                if (releaseMedia == null)
                    throw new TaskException(SystemMessage("RELEASE_MEDIA_NOT_FOUND"));

                releaseMedia.Type = update.Type;
                releaseMedia.Sku = update.Sku;
                releaseMedia.Upc = update.Upc;

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
