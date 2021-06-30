using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;

namespace SongtrackerPro.Tasks.PersonTasks
{
    public interface IUpdatePersonTask : ITask<Person, Nothing> { }

    public class UpdatePerson : TaskBase, IUpdatePersonTask
    {
        public UpdatePerson(ApplicationDbContext dbContext, IFormattingService formattingService)
        {
            _dbContext = dbContext;
            _formattingService = formattingService;
        }
        private readonly ApplicationDbContext _dbContext;
        private readonly IFormattingService _formattingService;

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
                person.MiddleName = string.IsNullOrWhiteSpace(update.MiddleName) ? null : update.MiddleName;
                person.LastName = update.LastName;
                person.NameSuffix = string.IsNullOrWhiteSpace(update.NameSuffix) ? null : update.NameSuffix;
                person.Email = string.IsNullOrWhiteSpace(update.Email) ? null : update.Email;
                person.Phone = _formattingService.FormatPhoneNumber(update.Phone);

                if (update.Address != null && !string.IsNullOrWhiteSpace(update.Address.Street))
                {
                    if (person.Address == null)
                    {
                        var address = update.Address;
                        var countryId = address.Country?.Id ?? address.CountryId;
                        var country = _dbContext.Countries.SingleOrDefault(c => c.Id == countryId);
                        address.Country = country;
                        _dbContext.Addresses.Add(address);
                        _dbContext.SaveChanges();

                        person.Address = address;
                    }
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
