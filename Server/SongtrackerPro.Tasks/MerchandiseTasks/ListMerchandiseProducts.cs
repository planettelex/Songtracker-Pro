using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.MerchandiseTasks
{
    public interface IListMerchandiseProductsTask : ITask<MerchandiseItem, List<MerchandiseProduct>> { }

    public class ListMerchandiseProducts : TaskBase, IListMerchandiseProductsTask
    {
        public ListMerchandiseProducts(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<List<MerchandiseProduct>> DoTask(MerchandiseItem merchandiseItem)
        {
            try
            {
                var merchandiseProducts = new List<MerchandiseProduct>();

                var publicationProducts = _dbContext.PublicationProducts
                    .Where(pp => pp.MerchandiseItemId == merchandiseItem.Id)
                    .Include(pp => pp.MerchandiseItem).ThenInclude(mi => mi.Artist)
                    .Include(pp => pp.MerchandiseItem).ThenInclude(mi => mi.Category)
                    .Include(pp => pp.Publication).ThenInclude(p => p.Publisher).ThenInclude(p => p.PerformingRightsOrganization)
                    .ToList();

                if (publicationProducts.Any())
                {
                    merchandiseProducts.AddRange(publicationProducts);
                    return new TaskResult<List<MerchandiseProduct>>(merchandiseProducts);
                }

                var releaseProducts = _dbContext.ReleaseProducts
                    .Where(rp => rp.MerchandiseItemId == merchandiseItem.Id)
                    .Include(rp => rp.MerchandiseItem).ThenInclude(mi => mi.Artist)
                    .Include(rp => rp.MerchandiseItem).ThenInclude(mi => mi.Category)
                    .Include(rp => rp.Release).ThenInclude(r => r.Artist)
                    .Include(rp => rp.Release).ThenInclude(r => r.Genre)
                    .Include(rp => rp.Release).ThenInclude(r => r.RecordLabel)
                    .ToList();

                if (releaseProducts.Any())
                {
                    merchandiseProducts.AddRange(releaseProducts);
                    return new TaskResult<List<MerchandiseProduct>>(merchandiseProducts);
                }

                var products = _dbContext.Products.Where(p => p.MerchandiseItemId == merchandiseItem.Id)
                    .Include(p => p.MerchandiseItem).ThenInclude(mi => mi.Artist)
                    .Include(p => p.MerchandiseItem).ThenInclude(mi => mi.Category)
                    .ToList();

                return new TaskResult<List<MerchandiseProduct>>(products);
            }
            catch (Exception e)
            {
                return new TaskResult<List<MerchandiseProduct>>(new TaskException(e));
            }
        }
    }
}
