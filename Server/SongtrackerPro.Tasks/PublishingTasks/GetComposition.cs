using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.PublishingTasks
{
    public interface IGetCompositionTask : ITask<int, Composition> { }

    public class GetComposition : TaskBase, IGetCompositionTask
    {
        public GetComposition(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<Composition> DoTask(int publicationId)
        {
            try
            {
                var composition = _dbContext.Compositions.Where(p => p.Id == publicationId)
                    .Include(p => p.Publisher).ThenInclude(b => b.Address).ThenInclude(a => a.Country)
                    .Include(p => p.Publisher).ThenInclude(b => b.PerformingRightsOrganization).ThenInclude(r => r.Country)
                    .SingleOrDefault();

                return new TaskResult<Composition>(composition);
            }
            catch (Exception e)
            {
                return new TaskResult<Composition>(new TaskException(e));
            }
        }
    }
}
