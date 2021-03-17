using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.PublishingTasks
{
    public interface IUpdatePublisherTask : ITask<Publisher, Nothing> { }

    public class UpdatePublisher : IUpdatePublisherTask
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
                    throw new TaskException("Publisher not found.");

                publisher.Name = update.Name;
                publisher.TaxId = update.TaxId;
                publisher.Email = update.Email;
                publisher.Phone = update.Phone;
                publisher.Address.Street = update.Address.Street;
                publisher.Address.City = update.Address.City;
                publisher.Address.Region = update.Address.Region;
                publisher.Address.PostalCode = update.Address.PostalCode;
                publisher.Address.Country = null;
                publisher.Address.CountryId = update.Address.Country.Id;
                publisher.PerformingRightsOrganization = null;
                publisher.PerformingRightsOrganizationId = update.PerformingRightsOrganization?.Id ?? update.PerformingRightsOrganizationId;
                publisher.PerformingRightsOrganizationPublisherNumber = update.PerformingRightsOrganizationPublisherNumber;
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
