using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.RecordLabelTasks
{
    public interface IUpdateRecordLabelTask : ITask<RecordLabel, Nothing> { }

    public class UpdateRecordLabel : IUpdateRecordLabelTask
    {
        public UpdateRecordLabel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<Nothing> DoTask(RecordLabel update)
        {
            try
            {
                var recordLabel = _dbContext.RecordLabels.Where(p => p.Id == update.Id)
                    .Include(p => p.Address)
                    .SingleOrDefault();

                if (recordLabel == null)
                    throw new TaskException("Record label not found.");

                recordLabel.Name = update.Name;
                recordLabel.TaxId = update.TaxId;
                recordLabel.Email = update.Email;
                recordLabel.Phone = update.Phone;
                recordLabel.Address.Street = update.Address.Street;
                recordLabel.Address.City = update.Address.City;
                recordLabel.Address.Region = update.Address.Region;
                recordLabel.Address.PostalCode = update.Address.PostalCode;
                recordLabel.Address.Country = null;
                recordLabel.Address.CountryId = update.Address.Country.Id;
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
