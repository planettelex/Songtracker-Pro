using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Queries;

namespace SongtrackerPro.Tasks.StorageItemTasks
{
    public interface IListPublisherContractsTask : ITask<PublisherContractsQuery, List<PublisherContract>> { }

    public class ListPublisherContracts : TaskBase, IListPublisherContractsTask
    {
        public ListPublisherContracts(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<List<PublisherContract>> DoTask(PublisherContractsQuery query)
        {
            try
            {
                List<PublisherContract> contracts = null;
                switch (query.ContractType)
                {
                    case PublisherContractsQuery.PublisherContractType.Template:
                        contracts = _dbContext.PublisherContracts.Where(pc => pc.PublisherId == query.PublisherId &&
                                                                              pc.IsTemplate == true)
                            .Include(pc => pc.Publication).ThenInclude(p => p.Publisher)
                            .Include(pc => pc.Composition).ThenInclude(c => c.Publisher)
                            .Include(pc => pc.Composition).ThenInclude(c => c.ExternalPublisher)
                            .Include(pc => pc.Artist)
                            .Include(pc => pc.Publisher)
                            .Include(pc => pc.Template)
                            .ToList();
                        break;
                    case PublisherContractsQuery.PublisherContractType.Client:
                        contracts = _dbContext.PublisherContracts.Where(pc => pc.PublisherId == query.PublisherId &&
                                                                              pc.IsTemplate == false)
                            .Include(pc => pc.Publication).ThenInclude(p => p.Publisher)
                            .Include(pc => pc.Composition).ThenInclude(c => c.Publisher)
                            .Include(pc => pc.Composition).ThenInclude(c => c.ExternalPublisher)
                            .Include(pc => pc.Artist)
                            .Include(pc => pc.Publisher)
                            .Include(pc => pc.Template)
                            .ToList();
                        break;
                }
                

                return new TaskResult<List<PublisherContract>>(contracts);
            }
            catch (Exception e)
            {
                return new TaskResult<List<PublisherContract>>(new TaskException(e));
            }
        }
    }
}
