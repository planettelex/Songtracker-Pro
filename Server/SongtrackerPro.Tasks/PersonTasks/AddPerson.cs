using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;

namespace SongtrackerPro.Tasks.PersonTasks
{
    public interface IAddPersonTask : ITask<Person, int?> { }

    public class AddPerson : TaskBase, IAddPersonTask
    {
        public AddPerson(ApplicationDbContext dbContext, IFormattingService formattingService)
        {
            _dbContext = dbContext;
            _formattingService = formattingService;
        }

        public AddPerson(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }

        private readonly ApplicationDbContext _dbContext;
        private readonly IFormattingService _formattingService;

        public ApplicationDbContext DbContext { get; }

        public TaskResult<int?> DoTask(Person person)
        {
            try
            {
                var address = person.Address;
                if (address != null && !string.IsNullOrWhiteSpace(address.Street))
                {
                    var countryId = address.Country?.Id ?? address.CountryId;
                    var country = _dbContext.Countries.SingleOrDefault(c => c.Id == countryId);
                    address.Country = country;
                    _dbContext.Addresses.Add(address);
                    _dbContext.SaveChanges();

                    person.AddressId = address.Id;
                }

                person.Address = null;
                person.MiddleName = string.IsNullOrWhiteSpace(person.MiddleName) ? null : person.MiddleName;
                person.NameSuffix = string.IsNullOrWhiteSpace(person.NameSuffix) ? null : person.NameSuffix;
                person.Email = string.IsNullOrWhiteSpace(person.Email) ? null : person.Email;
                person.Phone = _formattingService.FormatPhoneNumber(person.Phone);

                _dbContext.People.Add(person);
                _dbContext.SaveChanges();

                if (person.AddressId > 0)
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
