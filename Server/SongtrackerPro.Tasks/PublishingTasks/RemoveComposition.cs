using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.PublishingTasks
{
    public interface IRemoveCompositionTask : ITask<Composition, bool> { }

    public class RemoveComposition : TaskBase, IRemoveCompositionTask
    {
        public RemoveComposition(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<bool> DoTask(Composition composition)
        {
            try
            {
                var toRemove = _dbContext.Compositions.SingleOrDefault(c => c.Id == composition.Id);
                if (toRemove == null)
                    return new TaskResult<bool>(false);

                _dbContext.Compositions.Remove(toRemove);
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
