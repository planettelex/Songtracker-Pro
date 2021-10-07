using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.PublishingTasks
{
    public interface IRemoveCompositionAuthorTask : ITask<CompositionAuthor, bool> { }

    public class RemoveCompositionAuthor : TaskBase, IRemoveCompositionAuthorTask
    {
        public RemoveCompositionAuthor(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<bool> DoTask(CompositionAuthor compositionAuthor)
        {
            try
            {
                var toRemove = _dbContext.CompositionAuthors.SingleOrDefault(ca => ca.Id == compositionAuthor.Id);
                if (toRemove == null)
                    return new TaskResult<bool>(false);

                _dbContext.CompositionAuthors.Remove(toRemove);
                _dbContext.SaveChanges();

                return new TaskResult<bool>(true);
            }
            catch (Exception e)
            {
                return new TaskResult<bool>(new TaskException(e));
            }
        }
    }
}
