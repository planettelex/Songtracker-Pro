using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.PublishingTasks
{
    public interface IAddCompositionTask : ITask<Composition, int?> { }

    public class AddComposition : TaskBase, IAddCompositionTask
    {
        public AddComposition(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<int?> DoTask(Composition composition)
        {
            try
            {
                var publisherId = composition.Publisher?.Id ?? composition.PublisherId;
                var publisher = _dbContext.Publishers
                    .Where(p => p.Id == publisherId)
                    .Include(p => p.PerformingRightsOrganization).ThenInclude(r => r.Country)
                    .Include(p => p.Address).ThenInclude(a => a.Country)
                    .Single();

                composition.Publisher = null;
                composition.PublisherId = publisherId;

                _dbContext.Compositions.Add(composition);
                _dbContext.SaveChanges();

                composition.Publisher = publisher;

                return new TaskResult<int?>(composition.Id);
            }
            catch (Exception e)
            {
                return new TaskResult<int?>(new TaskException(e));
            }
        }
    }
}
