using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.RecordLabelTasks
{
    public interface IAddReleaseTask : ITask<Release, int?> { }

    public class AddRelease  : TaskBase, IAddReleaseTask
    {
        public AddRelease(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<int?> DoTask(Release release)
        {
            try
            {
                var recordLabelId = release.RecordLabel?.Id ?? release.RecordLabelId;
                var recordLabel = _dbContext.RecordLabels
                    .Where(rl => rl.Id == recordLabelId)
                    .Include(rl => rl.Address).ThenInclude(a => a.Country)
                    .Single();

                release.RecordLabel = null;
                release.RecordLabelId = recordLabelId;

                _dbContext.Releases.Add(release);
                _dbContext.SaveChanges();

                release.RecordLabel = recordLabel;

                return new TaskResult<int?>(release.Id);
            }
            catch (Exception e)
            {
                return new TaskResult<int?>(new TaskException(e));
            }
        }
    }
}
