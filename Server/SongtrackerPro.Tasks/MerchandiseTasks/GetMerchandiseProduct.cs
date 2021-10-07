using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.MerchandiseTasks
{
    public interface IGetMerchandiseProductTask : ITask<int, MerchandiseProduct> { }

    public class GetMerchandiseProduct : TaskBase, IGetMerchandiseProductTask
    {
        public GetMerchandiseProduct(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<MerchandiseProduct> DoTask(int merchandiseProductId)
        {
            try
            {
                var publicationProduct = _dbContext.PublicationProducts.Where(p => p.Id == merchandiseProductId)
                    .Include(p => p.MerchandiseItem).ThenInclude(mi => mi.Category)
                    .Include(p => p.MerchandiseItem).ThenInclude(mi => mi.Artist)
                    .Include(p => p.Publication).ThenInclude(p => p.Publisher).ThenInclude(p => p.PerformingRightsOrganization)
                    .SingleOrDefault();

                if (publicationProduct != null)
                    return new TaskResult<MerchandiseProduct>(publicationProduct);
                
                var releaseProduct = _dbContext.ReleaseProducts.Where(p => p.Id == merchandiseProductId)
                    .Include(p => p.MerchandiseItem).ThenInclude(mi => mi.Category)
                    .Include(p => p.MerchandiseItem).ThenInclude(mi => mi.Artist)
                    .Include(p => p.Release).ThenInclude(r => r.Artist)
                    .Include(p => p.Release).ThenInclude(r => r.Genre)
                    .Include(p => p.Release).ThenInclude(r => r.RecordLabel)
                    .SingleOrDefault();

                if (releaseProduct != null)
                    return new TaskResult<MerchandiseProduct>(releaseProduct);

                var product = _dbContext.Products.Where(p => p.Id == merchandiseProductId)
                    .Include(p => p.MerchandiseItem).ThenInclude(mi => mi.Category)
                    .Include(p => p.MerchandiseItem).ThenInclude(mi => mi.Artist)
                    .SingleOrDefault();

                return new TaskResult<MerchandiseProduct>(product);
            }
            catch (Exception e)
            {
                return new TaskResult<MerchandiseProduct>(new TaskException(e));
            }
        }
    }
}
