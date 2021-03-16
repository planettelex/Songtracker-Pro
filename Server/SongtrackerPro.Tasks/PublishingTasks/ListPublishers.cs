using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.PublishingTasks
{
    public interface IListPublishersTask : ITask<Nothing, List<Publisher>> { }

    public class ListPublishers : IListPublishersTask
    {
        public ListPublishers(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<List<Publisher>> DoTask(Nothing nothing)
        {
            try
            {
                var publishers = _dbContext.Publishers.Include(p => p.Address).ThenInclude(a => a.Country).Include(p => p.PerformingRightsOrganization).ToList();

                return new TaskResult<List<Publisher>>(publishers);
            }
            catch (Exception e)
            {
                return new TaskResult<List<Publisher>>(new TaskException(e));
            }
        }
    }
}