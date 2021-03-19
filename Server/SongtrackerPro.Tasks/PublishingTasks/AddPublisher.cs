using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.PublishingTasks
{
    public interface IAddPublisherTask : ITask<Publisher, int?> { }

    public class AddPublisher : TaskBase, IAddPublisherTask
    {
        public AddPublisher(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<int?> DoTask(Publisher publisher)
        {
            try
            {
                var address = publisher.Address;
                var countryId = address.Country?.Id ?? address.CountryId;
                var country = _dbContext.Countries.SingleOrDefault(c => c.Id == countryId);
                address.Country = country;
                _dbContext.Addresses.Add(address);
                _dbContext.SaveChanges();

                var proId = publisher.PerformingRightsOrganization?.Id ?? publisher.PerformingRightsOrganizationId;

                publisher.Address = null;
                publisher.AddressId = address.Id;
                publisher.PerformingRightsOrganization = null;
                publisher.PerformingRightsOrganizationId = proId;
                
                _dbContext.Publishers.Add(publisher);
                _dbContext.SaveChanges();

                publisher.Address = address;
                publisher.PerformingRightsOrganization = proId > 0 ?
                    _dbContext.PerformingRightsOrganizations.Where(p => p.Id == proId)
                    .Include(p => p.Country)
                    .SingleOrDefault() : null;

                return new TaskResult<int?>(publisher.Id);
            }
            catch (Exception e)
            {
                return new TaskResult<int?>(new TaskException(e));
            }
        }
    }
}
