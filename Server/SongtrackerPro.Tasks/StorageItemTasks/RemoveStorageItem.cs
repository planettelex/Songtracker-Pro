using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.StorageItemTasks
{
    public interface IRemoveStorageItemTask : ITask<StorageItem, bool> { }

    public class RemoveStorageItem : TaskBase, IRemoveStorageItemTask
    {
        public RemoveStorageItem(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<bool> DoTask(StorageItem storageItem)
        {
            try
            {
                var toRemove = _dbContext.StorageItems.SingleOrDefault(si => si.Uuid == storageItem.Uuid);
                if (toRemove == null)
                    return new TaskResult<bool>(false);

                _dbContext.StorageItems.Remove(toRemove);
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
