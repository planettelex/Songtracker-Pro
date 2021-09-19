using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.PublishingTasks
{
    public interface IGetPublicationTask : ITask<int, Publication> { }

    public class GetPublication : TaskBase, IGetPublicationTask
    {
        public GetPublication(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<Publication> DoTask(int publicationId)
        {
            try
            {
                var publication = _dbContext.Publications.Where(p => p.Id == publicationId)
                    .Include(p => p.Publisher).ThenInclude(b => b.Address).ThenInclude(a => a.Country)
                    .Include(p => p.Publisher).ThenInclude(b => b.PerformingRightsOrganization).ThenInclude(r => r.Country)
                    .SingleOrDefault();

                return new TaskResult<Publication>(publication);
            }
            catch (Exception e)
            {
                return new TaskResult<Publication>(new TaskException(e));
            }
        }
    }
}
