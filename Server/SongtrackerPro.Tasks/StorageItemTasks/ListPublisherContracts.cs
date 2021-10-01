using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.StorageItemTasks
{
    public interface IListPublisherContractsTask : ITask<Publisher, List<PublisherContract>> { }

    public class ListPublisherContracts : TaskBase, IListPublisherContractsTask
    {
        public ListPublisherContracts(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<List<PublisherContract>> DoTask(Publisher publisher)
        {
            try
            {
                var contracts = _dbContext.PublisherContracts.Where(pc => pc.PublisherId == publisher.Id)
                    .Include(pc => pc.Publication).ThenInclude(p => p.Publisher).ThenInclude(p => p.PerformingRightsOrganization)
                    .Include(pc => pc.Composition).ThenInclude(c => c.Publisher).ThenInclude(p => p.PerformingRightsOrganization)
                    .Include(pc => pc.Composition).ThenInclude(c => c.ExternalPublisher)
                    .Include(pc => pc.Artist)
                    .Include(pc => pc.Publisher).ThenInclude(p => p.PerformingRightsOrganization)
                    .Include(pc => pc.Template)
                    .ToList();

                return new TaskResult<List<PublisherContract>>(contracts);
            }
            catch (Exception e)
            {
                return new TaskResult<List<PublisherContract>>(new TaskException(e));
            }
        }
    }
}
