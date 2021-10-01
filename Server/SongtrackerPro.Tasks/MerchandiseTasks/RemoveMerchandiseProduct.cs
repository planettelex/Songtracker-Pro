using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.MerchandiseTasks
{
    public interface IRemoveMerchandiseProductTask : ITask<MerchandiseProduct, bool> { }

    public class RemoveMerchandiseProduct : TaskBase, IRemoveMerchandiseProductTask
    {
        public RemoveMerchandiseProduct(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<bool> DoTask(MerchandiseProduct merchandiseProduct)
        {
            try
            {
                var toRemove = _dbContext.Products.SingleOrDefault(p => p.Id == merchandiseProduct.Id);
                if (toRemove == null)
                    return new TaskResult<bool>(false);

                _dbContext.Products.Remove(toRemove);
                _dbContext.SaveChanges();

                return new TaskResult<bool>(true);
            }
            catch (Exception e)
            {
                return new TaskResult<bool>(new TaskException(e));
            }
        }
    }
}
