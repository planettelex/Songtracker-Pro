using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.PublishingTasks
{
    public interface IAddPublisherTask : ITask<Publisher, int?> { }

    public class AddPublisher : IAddPublisherTask
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
                var pro = _dbContext.PerformingRightsOrganizations.SingleOrDefault(p => p.Id == proId);

                publisher.Address = null;
                publisher.AddressId = address.Id;
                publisher.PerformingRightsOrganization = null;
                publisher.PerformingRightsOrganizationId = pro?.Id;
                
                _dbContext.Publishers.Add(publisher);
                _dbContext.SaveChanges();

                return new TaskResult<int?>(publisher.Id);
            }
            catch (Exception e)
            {
                return new TaskResult<int?>(new TaskException(e));
            }
        }
    }
}
