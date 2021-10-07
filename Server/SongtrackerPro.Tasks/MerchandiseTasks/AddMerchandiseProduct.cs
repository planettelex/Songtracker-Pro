using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.MerchandiseTasks
{
    public interface IAddMerchandiseProductTask : ITask<MerchandiseProduct, int?> { }

    public class AddMerchandiseProduct : TaskBase, IAddMerchandiseProductTask
    {
        public AddMerchandiseProduct(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<int?> DoTask(MerchandiseProduct merchandiseProduct)
        {
            try
            {
                var itemId = merchandiseProduct.MerchandiseItem?.Id ?? merchandiseProduct.MerchandiseItemId;
                var item = _dbContext.Merchandise.Single(mi => mi.Id == itemId);

                merchandiseProduct.MerchandiseItem = null;
                merchandiseProduct.MerchandiseItemId = itemId;

                if (merchandiseProduct is PublicationMerchandiseProduct publicationProduct)
                {
                    var publicationId = publicationProduct.Publication?.Id ?? publicationProduct.PublicationId;
                    var publication = _dbContext.Publications.Single(p => p.Id == publicationId);

                    publicationProduct.Publication = null;
                    publicationProduct.PublicationId = publicationId;

                    _dbContext.PublicationProducts.Add(publicationProduct);
                    _dbContext.SaveChanges();

                    publicationProduct.Publication = publication;
                }
                else if (merchandiseProduct is ReleaseMerchandiseProduct releaseProduct)
                {
                    var releaseId = releaseProduct.Release?.Id ?? releaseProduct.ReleaseId;
                    var release = _dbContext.Releases.Single(r => r.Id == releaseId);

                    releaseProduct.Release = null;
                    releaseProduct.ReleaseId = releaseId;

                    _dbContext.ReleaseProducts.Add(releaseProduct);
                    _dbContext.SaveChanges();

                    releaseProduct.Release = release;
                }
                else
                {
                    _dbContext.Products.Add(merchandiseProduct);
                    _dbContext.SaveChanges();
                }

                merchandiseProduct.MerchandiseItem = item;

                return new TaskResult<int?>(merchandiseProduct.Id);
            }
            catch (Exception e)
            {
                return new TaskResult<int?>(new TaskException(e));
            }
        }
    }
}
