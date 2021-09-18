using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.LegalEntityTasks
{
    public interface IGetLegalEntityTask : ITask<int, LegalEntity> { }

    public class GetLegalEntity : TaskBase, IGetLegalEntityTask
    {
        public GetLegalEntity(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<LegalEntity> DoTask(int legalEntityId)
        {
            try
            {
                var legalEntity = _dbContext.LegalEntities.Where(l => l.Id == legalEntityId)
                    .Include(l => l.Address).ThenInclude(a => a.Country)
                    .Include(l => l.LegalEntityServices).ThenInclude(les => les.Service)
                    .SingleOrDefault();

                return new TaskResult<LegalEntity>(legalEntity);
            }
            catch (Exception e)
            {
                return new TaskResult<LegalEntity>(new TaskException(e));
            }
        }
    }
}
