using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.PublishingTasks
{
    public interface IGetPublisherTask : ITask<int, Publisher> { }

    public class GetPublisher : IGetPublisherTask
    {
        public GetPublisher(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<Publisher> DoTask(int publisherId)
        {
            try
            {
                var publisher = _dbContext.Publishers.Where(p => p.Id == publisherId)
                    .Include(p => p.Address).ThenInclude(a => a.Country)
                    .Include(p => p.PerformingRightsOrganization).ThenInclude(a => a.Country)
                    .SingleOrDefault();

                return new TaskResult<Publisher>(publisher);
            }
            catch (Exception e)
            {
                return new TaskResult<Publisher>(new TaskException(e));
            }
        }
    }
}
