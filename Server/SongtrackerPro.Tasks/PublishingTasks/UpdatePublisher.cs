using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;

namespace SongtrackerPro.Tasks.PublishingTasks
{
    public interface IUpdatePublisherTask : ITask<Publisher, Nothing> { }

    public class UpdatePublisher : TaskBase, IUpdatePublisherTask
    {
        public UpdatePublisher(ApplicationDbContext dbContext, IFormattingService formattingService)
        {
            _dbContext = dbContext;
            _formattingService = formattingService;
        }
        private readonly ApplicationDbContext _dbContext;
        private readonly IFormattingService _formattingService;

        public TaskResult<Nothing> DoTask(Publisher update)
        {
            try
            {
                var publisher = _dbContext.Publishers.Where(p => p.Id == update.Id)
                    .Include(p => p.Address)
                    .SingleOrDefault();

                if (publisher == null)
                    throw new TaskException(SystemMessage("PUBLISHER_NOT_FOUND"));

                publisher.Name = update.Name;
                publisher.TaxId = _formattingService.FormatTaxId(update.TaxId);
                publisher.Email = update.Email;
                publisher.Phone = _formattingService.FormatPhoneNumber(update.Phone);

                if (update.Address != null)
                {
                    if (publisher.Address == null)
                    {
                        var address = update.Address;
                        var countryId = address.Country?.Id ?? address.CountryId;
                        var country = _dbContext.Countries.SingleOrDefault(c => c.Id == countryId);
                        address.Country = country;
                        _dbContext.Addresses.Add(address);
                        _dbContext.SaveChanges();
                        publisher.Address = address;
                    }
                    publisher.Address.Street = update.Address.Street;
                    publisher.Address.City = update.Address.City;
                    publisher.Address.Region = update.Address.Region;
                    publisher.Address.PostalCode = update.Address.PostalCode;

                    publisher.Address.CountryId = update.Address.Country?.Id;
                    if (publisher.Address.CountryId.HasValue)
                    {
                        var country = _dbContext.Countries.SingleOrDefault(c => c.Id == publisher.Address.CountryId);
                        publisher.Address.Country = country ?? throw new TaskException(SystemMessage("COUNTRY_NOT_FOUND"));
                    }
                }

                publisher.PerformingRightsOrganizationId = update.PerformingRightsOrganization?.Id;
                if (publisher.PerformingRightsOrganizationId.HasValue)
                {
                    var pro = _dbContext.PerformingRightsOrganizations.SingleOrDefault(r => r.Id == publisher.PerformingRightsOrganizationId);
                    publisher.PerformingRightsOrganization = pro ?? throw new TaskException(SystemMessage("PRO_NOT_FOUND"));
                }
                publisher.PerformingRightsOrganizationPublisherNumber = update.PerformingRightsOrganizationPublisherNumber;

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
