using System;
using System.Collections.Generic;
using System.Text;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.PublishingTasks
{
    public interface IAddPublicationAuthorTask : ITask<PublicationAuthor, int?> { }

    public class AddPublicationAuthor : TaskBase, IAddPublicationAuthorTask
    {
        public AddPublicationAuthor(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<int?> DoTask(PublicationAuthor publicationAuthor)
        {
            try
            {
                var publicationId = publicationAuthor.Publication?.Id ?? publicationAuthor.PublicationId;
                var authorId = publicationAuthor.Author?.Id ?? publicationAuthor.PersonId;

                publicationAuthor.Author = null;
                publicationAuthor.PersonId = authorId;
                publicationAuthor.Publication = null;
                publicationAuthor.PublicationId = publicationId;

                _dbContext.PublicationAuthors.Add(publicationAuthor);
                _dbContext.SaveChanges();

                return new TaskResult<int?>(publicationAuthor.Id);
            }
            catch (Exception e)
            {
                return new TaskResult<int?>(new TaskException(e));
            }
        }
    }
}
