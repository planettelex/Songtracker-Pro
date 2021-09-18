using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.LegalEntityTasks
{
    public interface IListLegalEntitiesTask : ITask<Nothing, List<LegalEntity>> { }

    public class ListLegalEntities : TaskBase, IListLegalEntitiesTask
    {
        public ListLegalEntities(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<List<LegalEntity>> DoTask(Nothing nothing)
        {
            try
            {
                var legalEntities = _dbContext.LegalEntities
                    .Include(p => p.Address).ThenInclude(a => a.Country)
                    .Include(p => p.LegalEntityServices).ThenInclude(les => les.Service)
                    .ToList();

                return new TaskResult<List<LegalEntity>>(legalEntities);
            }
            catch (Exception e)
            {
                return new TaskResult<List<LegalEntity>>(new TaskException(e));
            }
        }
    }
}
