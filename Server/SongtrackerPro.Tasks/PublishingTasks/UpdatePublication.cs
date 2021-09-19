using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.PublishingTasks
{
    public interface IUpdatePublicationTask : ITask<Publication, Nothing> { }

    public class UpdatePublication : TaskBase, IUpdatePublicationTask
    {
        public UpdatePublication(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<Nothing> DoTask(Publication update)
        {
            try
            {
                var publication = _dbContext.Publications
                    .SingleOrDefault(p => p.Id == update.Id);

                if (publication == null)
                    throw new TaskException(SystemMessage("PUBLICATION_NOT_FOUND"));

                publication.Title = update.Title;
                publication.CatalogNumber = update.CatalogNumber;
                publication.Isbn = update.Isbn;
                publication.CopyrightedOn = update.CopyrightedOn;

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
