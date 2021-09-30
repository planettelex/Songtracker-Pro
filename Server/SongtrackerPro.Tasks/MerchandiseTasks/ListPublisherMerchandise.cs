using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.MerchandiseTasks
{
    public interface IListPublisherMerchandiseTask : ITask<Publisher, List<PublisherMerchandiseItem>> { }

    public class ListPublisherMerchandise : TaskBase, IListPublisherMerchandiseTask
    {
        public ListPublisherMerchandise(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<List<PublisherMerchandiseItem>> DoTask(Publisher publisher)
        {
            try
            {
                var merchandiseItems = _dbContext.PublisherMerchandise.Where(mi => mi.PublisherId == publisher.Id)
                    .Include(mi => mi.Artist)
                    .Include(mi => mi.Category)
                    .Include(mi => mi.Publisher)
                    .ToList();

                return new TaskResult<List<PublisherMerchandiseItem>>(merchandiseItems);
            }
            catch (Exception e)
            {
                return new TaskResult<List<PublisherMerchandiseItem>>(new TaskException(e));
            }
        }
    }
}
