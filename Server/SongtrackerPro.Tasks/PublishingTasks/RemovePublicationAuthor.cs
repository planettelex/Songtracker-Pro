using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.PublishingTasks
{
    public interface IRemovePublicationAuthorTask : ITask<PublicationAuthor, bool> { }

    public class RemovePublicationAuthor : TaskBase, IRemovePublicationAuthorTask
    {
        public RemovePublicationAuthor(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<bool> DoTask(PublicationAuthor publicationAuthor)
        {
            try
            {
                var toRemove = _dbContext.PublicationAuthors.SingleOrDefault(pa => pa.Id == publicationAuthor.Id);
                if (toRemove == null)
                    return new TaskResult<bool>(false);

                _dbContext.PublicationAuthors.Remove(toRemove);
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
