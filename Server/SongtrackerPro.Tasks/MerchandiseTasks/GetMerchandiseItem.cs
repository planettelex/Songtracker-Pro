using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.MerchandiseTasks
{
    public interface IGetMerchandiseItemTask : ITask<int, MerchandiseItem> { }

    public class GetMerchandiseItem : TaskBase, IGetMerchandiseItemTask
    {
        public GetMerchandiseItem(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<MerchandiseItem> DoTask(int merchandiseItemId)
        {
            try
            {
                var publisherMerchandiseItem = _dbContext.PublisherMerchandise.Where(mi => mi.Id == merchandiseItemId)
                    .Include(mi => mi.Artist)
                    .Include(mi => mi.Category)
                    .Include(mi => mi.Publisher)
                    .SingleOrDefault();

                if (publisherMerchandiseItem != null)
                    return new TaskResult<MerchandiseItem>(publisherMerchandiseItem);
                
                var recordLabelMerchandiseItem = _dbContext.RecordLabelMerchandise.Where(mi => mi.Id == merchandiseItemId)
                    .Include(mi => mi.Artist)
                    .Include(mi => mi.Category)
                    .Include(mi => mi.RecordLabel)
                    .SingleOrDefault();

                if (recordLabelMerchandiseItem != null)
                    return new TaskResult<MerchandiseItem>(recordLabelMerchandiseItem);
                
                var merchandiseItem = _dbContext.Merchandise.Where(mi => mi.Id == merchandiseItemId)
                    .Include(mi => mi.Artist)
                    .Include(mi => mi.Category)
                    .SingleOrDefault();

                return new TaskResult<MerchandiseItem>(merchandiseItem);
            }
            catch (Exception e)
            {
                return new TaskResult<MerchandiseItem>(new TaskException(e));
            }
        }
    }
}
