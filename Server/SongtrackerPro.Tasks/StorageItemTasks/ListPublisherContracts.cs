using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Enums;
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
                switch (query.QueryType)
                {
                    case PublisherContractsQuery.PublisherContractQueryType.Template:
                        contracts = _dbContext.PublisherContracts.Where(pc => pc.PublisherId == query.PublisherId &&
                                                                              pc.IsTemplate == true)
                            .Include(pc => pc.Publication).ThenInclude(p => p.Publisher)
                            .Include(pc => pc.Composition).ThenInclude(c => c.Publisher)
                            .Include(pc => pc.Composition).ThenInclude(c => c.ExternalPublisher)
                            .Include(pc => pc.Artist)
                            .Include(pc => pc.Publisher)
                            .ToList();
                        break;
                    case PublisherContractsQuery.PublisherContractQueryType.Client:
                        var clients = _dbContext.Users.Where(u => u.UserType == UserType.SystemUser && u.PublisherId == query.PublisherId)
                            .ToList();
                        var clientIds = clients.Select(c => c.Id).ToList();
                        contracts = _dbContext.PublisherContracts.Where(pc => pc.PublisherId == query.PublisherId &&
                                                                              pc.IsTemplate == false)
                            .Include(pc => pc.Publication).ThenInclude(p => p.Publisher)
                            .Include(pc => pc.Composition).ThenInclude(c => c.Publisher)
                            .Include(pc => pc.Composition).ThenInclude(c => c.ExternalPublisher)
                            .Include(pc => pc.Artist)
                            .Include(pc => pc.Publisher)
                            .Include(pc => pc.Template)
                            .Include(pc => pc.Parties)
                            .Join(_dbContext.ContractParties, c => c.Uuid, cp => cp.ContractId, (c, cp) => new { c, cp })
                            .Include(j => j.cp.LegalEntity)
                            .Where(j => clientIds.Contains(j.cp.LegalEntityId.Value))
                            .Select(j => j.c)
                            .ToList();
                        break;
                    case PublisherContractsQuery.PublisherContractQueryType.Publication:
                        contracts = _dbContext.PublisherContracts.Where(pc => pc.PublisherId == query.PublisherId &&
                                                                              pc.PublicationId != null &&
                                                                              pc.IsTemplate == false)
                            .Include(pc => pc.Publication).ThenInclude(p => p.Publisher)
                            .Include(pc => pc.Composition).ThenInclude(c => c.Publisher)
                            .Include(pc => pc.Composition).ThenInclude(c => c.ExternalPublisher)
                            .Include(pc => pc.Artist)
                            .Include(pc => pc.Publisher)
                            .Include(pc => pc.Template)
                            .ToList();
                        break;
                    case PublisherContractsQuery.PublisherContractQueryType.Composition:
                        contracts = _dbContext.PublisherContracts.Where(pc => pc.PublisherId == query.PublisherId &&
                                                                              pc.CompositionId != null &&
                                                                              pc.IsTemplate == false)
                            .Include(pc => pc.Publication).ThenInclude(p => p.Publisher)
                            .Include(pc => pc.Composition).ThenInclude(c => c.Publisher)
                            .Include(pc => pc.Composition).ThenInclude(c => c.ExternalPublisher)
                            .Include(pc => pc.Artist)
                            .Include(pc => pc.Publisher)
                            .Include(pc => pc.Template)
                            .ToList();
                        break;
                    case PublisherContractsQuery.PublisherContractQueryType.General:
                        contracts = _dbContext.PublisherContracts.Where(pc => pc.PublisherId == query.PublisherId &&
                                                                              pc.CompositionId == null &&
                                                                              pc.PublicationId == null &&
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
