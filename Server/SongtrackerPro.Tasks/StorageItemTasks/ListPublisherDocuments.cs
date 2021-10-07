using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.StorageItemTasks
{
    public interface IListPublisherDocumentsTask : ITask<Publisher, List<Document>> { }

    public class ListPublisherDocuments : TaskBase, IListPublisherDocumentsTask
    {
        public ListPublisherDocuments(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<List<Document>> DoTask(Publisher publisher)
        {
            try
            {
                var documents = _dbContext.Documents.Where(d => d.PublisherId == publisher.Id)
                    .Include(d => d.Artist)
                    .Include(d => d.Publisher).ThenInclude(p => p.PerformingRightsOrganization)
                    .Include(d => d.RecordLabel)
                    .ToList();

                return new TaskResult<List<Document>>(documents);
            }
            catch (Exception e)
            {
                return new TaskResult<List<Document>>(new TaskException(e));
            }
        }
    }
}
