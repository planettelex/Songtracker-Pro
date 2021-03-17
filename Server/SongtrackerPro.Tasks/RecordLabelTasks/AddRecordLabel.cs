using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.RecordLabelTasks
{
    public interface IAddRecordLabelTask : ITask<RecordLabel, int?> { }

    public class AddRecordLabel : IAddRecordLabelTask
    {
        public AddRecordLabel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<int?> DoTask(RecordLabel recordLabel)
        {
            try
            {
                var address = recordLabel.Address;
                var countryId = address.Country?.Id ?? address.CountryId;
                var country = _dbContext.Countries.SingleOrDefault(c => c.Id == countryId);
                address.Country = country;
                _dbContext.Addresses.Add(address);
                _dbContext.SaveChanges();

                recordLabel.Address = null;
                recordLabel.AddressId = address.Id;

                _dbContext.RecordLabels.Add(recordLabel);
                _dbContext.SaveChanges();

                recordLabel.Address = address;

                return new TaskResult<int?>(recordLabel.Id);
            }
            catch (Exception e)
            {
                return new TaskResult<int?>(new TaskException(e));
            }
        }
    }
}
