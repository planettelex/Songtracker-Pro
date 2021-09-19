using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.PublishingTasks
{
    public interface IRemovePublicationTask : ITask<Publication, bool> { }

    public class RemovePublication : TaskBase, IRemovePublicationTask
    {
        public RemovePublication(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<bool> DoTask(Publication publication)
        {
            try
            {
                var toRemove = _dbContext.Publications.SingleOrDefault(p => p.Id == publication.Id);
                if (toRemove == null)
                    return new TaskResult<bool>(false);

                _dbContext.Publications.Remove(toRemove);
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
