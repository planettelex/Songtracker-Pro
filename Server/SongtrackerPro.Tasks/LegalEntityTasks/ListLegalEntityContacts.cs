using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.LegalEntityTasks
{
    public interface IListLegalEntityContactsTask : ITask<LegalEntity, List<LegalEntityContact>> { }

    public class ListLegalEntityContacts : TaskBase, IListLegalEntityContactsTask
    {
        public ListLegalEntityContacts(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<List<LegalEntityContact>> DoTask(LegalEntity legalEntity)
        {
            try
            {
                var legalEntityContacts = _dbContext.LegalEntityContacts.Where(le => le.LegalEntityId == legalEntity.Id)
                    .Include(le => le.Contact).ThenInclude(c => c.Address).ThenInclude(a => a.Country)
                    .ToList();

                return new TaskResult<List<LegalEntityContact>>(legalEntityContacts);
            }
            catch (Exception e)
            {
                return new TaskResult<List<LegalEntityContact>>(new TaskException(e));
            }
        }
    }
}
