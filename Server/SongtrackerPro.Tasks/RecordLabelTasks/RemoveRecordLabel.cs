using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.RecordLabelTasks
{
    public interface IRemoveRecordLabelTask : ITask<RecordLabel, bool> { }

    public class RemoveRecordLabel : IRemoveRecordLabelTask
    {
        public RemoveRecordLabel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<bool> DoTask(RecordLabel recordLabel)
        {
            try
            {
                var toRemove = _dbContext.RecordLabels.SingleOrDefault(p => p.Id == recordLabel.Id);
                if (toRemove == null)
                    return new TaskResult<bool>(false);

                var addressId = toRemove.AddressId;
                _dbContext.RecordLabels.Remove(toRemove);
                _dbContext.SaveChanges();

                if (addressId.HasValue)
                {
                    var addressToRemove = _dbContext.Addresses.SingleOrDefault(a => a.Id == addressId.Value);
                    if (addressToRemove != null)
                    {
                        _dbContext.Addresses.Remove(addressToRemove);
                        _dbContext.SaveChanges();
                    }
                }

                return new TaskResult<bool>(true);
            }
            catch (Exception e)
            {
                return new TaskResult<bool>(new TaskException(e));
            }
        }
    }
}
