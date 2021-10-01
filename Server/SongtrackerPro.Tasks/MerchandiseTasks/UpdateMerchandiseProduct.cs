using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.MerchandiseTasks
{
    public interface IUpdateMerchandiseProductTask : ITask<MerchandiseProduct, Nothing> { }

    public class UpdateMerchandiseProduct : TaskBase, IUpdateMerchandiseProductTask
    {
        public UpdateMerchandiseProduct(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<Nothing> DoTask(MerchandiseProduct update)
        {
            try
            {
                var product = _dbContext.Products.Single(p => p.Id == update.Id);
                product.Color = update.Color;
                product.ColorName = update.ColorName;
                product.Description = update.Description;
                product.Name = update.Name;
                product.Size = update.Size;
                product.Sku = update.Sku;
                product.Upc = update.Upc;
                _dbContext.SaveChanges();

                if (update is PublicationMerchandiseProduct publicationProductUpdate)
                {
                    var  publicationProduct = _dbContext.PublicationProducts.Single(pp => pp.Id == publicationProductUpdate.Id);
                    publicationProduct.IssueNumber = publicationProductUpdate.IssueNumber;
                    _dbContext.SaveChanges();
                }
                else if (update is ReleaseMerchandiseProduct releaseProductUpdate)
                {
                    var  releaseProduct = _dbContext.ReleaseProducts.Single(rp => rp.Id == releaseProductUpdate.Id);
                    releaseProduct.MediaType = releaseProductUpdate.MediaType;
                    _dbContext.SaveChanges();
                }

                return new TaskResult<Nothing>(true);
            }
            catch (Exception e)
            {
                return new TaskResult<Nothing>(new TaskException(e));
            }
        }
    }
}
