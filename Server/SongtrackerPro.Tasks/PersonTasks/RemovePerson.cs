using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.PersonTasks
{
    public interface IRemovePersonTask : ITask<Person, bool> { }

    public class RemovePerson : TaskBase, IRemovePersonTask
    {
        public RemovePerson(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<bool> DoTask(Person person)
        {
            try
            {
                var toRemove = _dbContext.People.SingleOrDefault(p => p.Id == person.Id);
                if (toRemove == null)
                    return new TaskResult<bool>(false);

                var addressId = toRemove.AddressId;
                _dbContext.People.Remove(toRemove);
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
