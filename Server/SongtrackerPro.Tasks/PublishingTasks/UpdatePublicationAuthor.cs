using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.PublishingTasks
{
    public interface IUpdatePublicationAuthorTask : ITask<PublicationAuthor, Nothing> { }

    public class UpdatePublicationAuthor : TaskBase, IUpdatePublicationAuthorTask
    {
        public UpdatePublicationAuthor(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<Nothing> DoTask(PublicationAuthor update)
        {
            try
            {
                var publicationAuthor = _dbContext.PublicationAuthors.SingleOrDefault(pa => pa.Id == update.Id);

                if (publicationAuthor == null)
                    throw new TaskException(SystemMessage("PUBLICATION_AUTHOR_NOT_FOUND"));

                publicationAuthor.PersonId = update.Author?.Id ?? update.PersonId;
                publicationAuthor.Author = _dbContext.People.Single(p => p.Id == publicationAuthor.PersonId);
                publicationAuthor.OwnershipPercentage = update.OwnershipPercentage;
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
