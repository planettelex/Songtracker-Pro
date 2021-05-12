using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.PersonTasks
{
    public interface IUpdatePersonTask : ITask<Person, Nothing> { }

    public class UpdatePerson : TaskBase, IUpdatePersonTask
    {
        public UpdatePerson(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<Nothing> DoTask(Person update)
        {
            try
            {
                var person = _dbContext.People.Where(p => p.Id == update.Id)
                    .Include(p => p.Address)
                    .SingleOrDefault();

                if (person == null)
                    throw new TaskException(SystemMessage("PERSON_NOT_FOUND"));

                person.FirstName = update.FirstName;
                person.MiddleName = update.MiddleName;
                person.LastName = update.LastName;
                person.NameSuffix = update.NameSuffix;
                person.Email = update.Email;
                person.Phone = update.Phone;
                person.Address.Street = update.Address.Street;
                person.Address.City = update.Address.City;
                person.Address.Region = update.Address.Region;
                person.Address.PostalCode = update.Address.PostalCode;

                person.Address.CountryId = update.Address.Country?.Id;
                if (person.Address.CountryId.HasValue)
                {
                    var country = _dbContext.Countries.SingleOrDefault(c => c.Id == person.Address.CountryId);
                    person.Address.Country = country ?? throw new TaskException(SystemMessage("COUNTRY_NOT_FOUND"));
                }

                _dbContext.SaveChanges();

                return new TaskResult<Nothing>(true);
            }
            catch (Exception e)
            {
                return new TaskResult<Nothing>(new TaskException(e));
            }
        }
    }
}
