using System;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.PublishingTasks
{
    public interface IAddCompositionAuthorTask : ITask<CompositionAuthor, int?> { }

    public class AddCompositionAuthor : TaskBase, IAddCompositionAuthorTask
    {
        public AddCompositionAuthor(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<int?> DoTask(CompositionAuthor compositionAuthor)
        {
            try
            {
                var compositionId = compositionAuthor.Composition?.Id ?? compositionAuthor.CompositionId;
                var authorId = compositionAuthor.Author?.Id ?? compositionAuthor.PersonId;

                compositionAuthor.Author = null;
                compositionAuthor.PersonId = authorId;
                compositionAuthor.Composition = null;
                compositionAuthor.CompositionId = compositionId;

                _dbContext.CompositionAuthors.Add(compositionAuthor);
                _dbContext.SaveChanges();

                return new TaskResult<int?>(compositionAuthor.Id);
            }
            catch (Exception e)
            {
                return new TaskResult<int?>(new TaskException(e));
            }
        }
    }
}
