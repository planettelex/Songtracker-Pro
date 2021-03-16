using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.PublishingTasks
{
    public interface IRemovePublisherTask : ITask<Publisher, bool> { }

    public class RemovePublisher : IRemovePublisherTask
    {
        public RemovePublisher(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<bool> DoTask(Publisher publisher)
        {
            try
            {
                var toRemove = _dbContext.Publishers.SingleOrDefault(p => p.Id == publisher.Id);
                if (toRemove == null)
                    return new TaskResult<bool>(false);

                var addressId = toRemove.AddressId;
                _dbContext.Publishers.Remove(toRemove);
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
