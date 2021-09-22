using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.RecordLabelTasks
{
    public interface IUpdateReleaseTask : ITask<Release, Nothing> { }

    public class UpdateRelease : TaskBase, IUpdateReleaseTask
    {
        public UpdateRelease(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<Nothing> DoTask(Release update)
        {
            try
            {
                var release = _dbContext.Releases
                    .SingleOrDefault(r => r.Id == update.Id);

                if (release == null)
                    throw new TaskException(SystemMessage("RELEASE_NOT_FOUND"));

                release.CatalogNumber = update.CatalogNumber;
                release.Title = update.Title;
                release.Type = update.Type;

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
