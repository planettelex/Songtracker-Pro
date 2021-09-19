using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.PublishingTasks
{
    public interface IUpdateCompositionAuthorTask : ITask<CompositionAuthor, Nothing> { }

    public class UpdateCompositionAuthor : TaskBase, IUpdateCompositionAuthorTask
    {
        public UpdateCompositionAuthor(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<Nothing> DoTask(CompositionAuthor update)
        {
            try
            {
                var compositionAuthor = _dbContext.CompositionAuthors.SingleOrDefault(ca => ca.Id == update.Id);

                if (compositionAuthor == null)
                    throw new TaskException(SystemMessage("COMPOSITION_AUTHOR_NOT_FOUND"));

                compositionAuthor.PersonId = update.Author?.Id ?? update.PersonId;
                compositionAuthor.Author = _dbContext.People.Single(p => p.Id == compositionAuthor.PersonId);
                compositionAuthor.OwnershipPercentage = update.OwnershipPercentage;
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
