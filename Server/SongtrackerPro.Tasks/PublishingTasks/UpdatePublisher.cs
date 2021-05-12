using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.PublishingTasks
{
    public interface IUpdatePublisherTask : ITask<Publisher, Nothing> { }

    public class UpdatePublisher : TaskBase, IUpdatePublisherTask
    {
        public UpdatePublisher(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

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
                publisher.TaxId = update.TaxId;
                publisher.Email = update.Email;
                publisher.Phone = update.Phone;
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
