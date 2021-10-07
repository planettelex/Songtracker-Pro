using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.PublishingTasks
{
    public interface IListPublicationAuthorsTask : ITask<Publication, List<PublicationAuthor>> { }

    public class ListPublicationAuthors : TaskBase, IListPublicationAuthorsTask
    {
        public ListPublicationAuthors(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<List<PublicationAuthor>> DoTask(Publication publication)
        {
            try
            {
                var publicationAuthors = _dbContext.PublicationAuthors.Where(pa => pa.PublicationId == publication.Id)
                    .Include(pa => pa.Author).ThenInclude(p => p.Address).ThenInclude(a => a.Country)
                    .OrderByDescending(pa => pa.OwnershipPercentage)
                    .ToList();

                return new TaskResult<List<PublicationAuthor>>(publicationAuthors);
            }
            catch (Exception e)
            {
                return new TaskResult<List<PublicationAuthor>>(new TaskException(e));
            }
        }
    }
}
