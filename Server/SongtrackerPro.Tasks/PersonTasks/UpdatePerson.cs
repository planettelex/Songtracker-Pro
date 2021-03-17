using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.PersonTasks
{
    public interface IUpdatePersonTask : ITask<Person, Nothing> { }

    public class UpdatePerson : IUpdatePersonTask
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
                    throw new TaskException("Record label not found.");

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
                person.Address.Country = null;
                person.Address.CountryId = update.Address.Country.Id;
                _dbContext.SaveChanges();

                return new TaskResult<Nothing>(null as Nothing);
            }
            catch (Exception e)
            {
                return new TaskResult<Nothing>(new TaskException(e));
            }
        }
    }
}
