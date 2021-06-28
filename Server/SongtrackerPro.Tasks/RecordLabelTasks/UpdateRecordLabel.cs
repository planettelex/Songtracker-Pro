using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;

namespace SongtrackerPro.Tasks.RecordLabelTasks
{
    public interface IUpdateRecordLabelTask : ITask<RecordLabel, Nothing> { }

    public class UpdateRecordLabel : TaskBase, IUpdateRecordLabelTask
    {
        public UpdateRecordLabel(ApplicationDbContext dbContext, IFormattingService formattingService)
        {
            _dbContext = dbContext;
            _formattingService = formattingService;
        }
        private readonly ApplicationDbContext _dbContext;
        private readonly IFormattingService _formattingService;

        public TaskResult<Nothing> DoTask(RecordLabel update)
        {
            try
            {
                var recordLabel = _dbContext.RecordLabels.Where(l => l.Id == update.Id)
                    .Include(p => p.Address)
                    .SingleOrDefault();

                if (recordLabel == null)
                    throw new TaskException(SystemMessage("RECORD_LABEL_NOT_FOUND"));

                recordLabel.Name = update.Name;
                recordLabel.TaxId = _formattingService.FormatTaxId(update.TaxId);
                recordLabel.Email = update.Email;
                recordLabel.Phone = _formattingService.FormatPhoneNumber(update.Phone);
                recordLabel.Address.Street = update.Address.Street;
                recordLabel.Address.City = update.Address.City;
                recordLabel.Address.Region = update.Address.Region;
                recordLabel.Address.PostalCode = update.Address.PostalCode;

                recordLabel.Address.CountryId = update.Address.Country?.Id;
                if (recordLabel.Address.CountryId.HasValue)
                {
                    var country = _dbContext.Countries.SingleOrDefault(c => c.Id == recordLabel.Address.CountryId);
                    recordLabel.Address.Country = country ?? throw new TaskException(SystemMessage("COUNTRY_NOT_FOUND"));
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
