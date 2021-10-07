using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.PublishingTasks
{
    public interface IAddPublicationTask : ITask<Publication, int?> { }

    public class AddPublication : TaskBase, IAddPublicationTask
    {
        public AddPublication(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<int?> DoTask(Publication publication)
        {
            try
            {
                var publisherId = publication.Publisher?.Id ?? publication.PublisherId;
                var publisher = _dbContext.Publishers
                    .Where(p => p.Id == publisherId)
                    .Include(p => p.PerformingRightsOrganization).ThenInclude(r => r.Country)
                    .Include(p => p.Address).ThenInclude(a => a.Country)
                    .SingleOrDefault();

                publication.Publisher = null;
                publication.PublisherId = publisherId;

                _dbContext.Publications.Add(publication);
                _dbContext.SaveChanges();

                publication.Publisher = publisher;

                return new TaskResult<int?>(publication.Id);
            }
            catch (Exception e)
            {
                return new TaskResult<int?>(new TaskException(e));
            }
        }
    }
}
