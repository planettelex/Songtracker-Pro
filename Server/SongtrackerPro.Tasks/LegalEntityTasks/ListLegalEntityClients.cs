using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.LegalEntityTasks
{
    public interface IListLegalEntityClientsTask : ITask<LegalEntity, List<LegalEntityClient>> { }

    public class ListLegalEntityClients : TaskBase, IListLegalEntityClientsTask
    {
        public ListLegalEntityClients(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<List<LegalEntityClient>> DoTask(LegalEntity legalEntity)
        {
            try
            {
                var legalEntityClients = _dbContext.LegalEntityClients.Where(le => le.LegalEntityId == legalEntity.Id)
                    .Include(le => le.Person).ThenInclude(c => c.Address).ThenInclude(a => a.Country)
                    .ToList();

                return new TaskResult<List<LegalEntityClient>>(legalEntityClients);
            }
            catch (Exception e)
            {
                return new TaskResult<List<LegalEntityClient>>(new TaskException(e));
            }
        }
    }
}
