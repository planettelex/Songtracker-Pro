using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.PersonTasks
{
    public interface IAddPersonTask : ITask<Person, int?> { }

    public class AddPerson : TaskBase, IAddPersonTask
    {
        public AddPerson(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<int?> DoTask(Person person)
        {
            try
            {
                var address = person.Address;
                var countryId = address.Country?.Id ?? address.CountryId;
                var country = _dbContext.Countries.SingleOrDefault(c => c.Id == countryId);
                address.Country = country;
                _dbContext.Addresses.Add(address);
                _dbContext.SaveChanges();

                person.Address = null;
                person.AddressId = address.Id;

                _dbContext.People.Add(person);
                _dbContext.SaveChanges();

                person.Address = address;

                return new TaskResult<int?>(person.Id);
            }
            catch (Exception e)
            {
                return new TaskResult<int?>(new TaskException(e));
            }
        }
    }
}
