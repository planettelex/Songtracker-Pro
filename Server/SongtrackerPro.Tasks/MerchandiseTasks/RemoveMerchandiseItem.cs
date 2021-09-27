using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.MerchandiseTasks
{
    public interface IRemoveMerchandiseItemTask : ITask<MerchandiseItem, bool> { }

    public class RemoveMerchandiseItem : TaskBase, IRemoveMerchandiseItemTask
    {
        public RemoveMerchandiseItem(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<bool> DoTask(MerchandiseItem merchandiseItem)
        {
            try
            {
                var toRemove = _dbContext.MerchandiseItems.SingleOrDefault(mi => mi.Id == merchandiseItem.Id);
                if (toRemove == null)
                    return new TaskResult<bool>(false);

                _dbContext.MerchandiseItems.Remove(toRemove);
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
