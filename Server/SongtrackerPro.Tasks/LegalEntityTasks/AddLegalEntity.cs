using System;
using System.Collections.Generic;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;

namespace SongtrackerPro.Tasks.LegalEntityTasks
{
    public interface IAddLegalEntityTask : ITask<LegalEntity, int?> { }

    public class AddLegalEntity : TaskBase, IAddLegalEntityTask
    {
        public AddLegalEntity(ApplicationDbContext dbContext, IFormattingService formattingService)
        {
            _dbContext = dbContext;
            _formattingService = formattingService;
        }
        private readonly ApplicationDbContext _dbContext;
        private readonly IFormattingService _formattingService;

        public TaskResult<int?> DoTask(LegalEntity legalEntity)
        {
            try
            {
                var address = legalEntity.Address;
                var countryId = address.Country?.Id ?? address.CountryId;
                var country = _dbContext.Countries.SingleOrDefault(c => c.Id == countryId);
                address.Country = country;
                _dbContext.Addresses.Add(address);
                _dbContext.SaveChanges();

                legalEntity.Address = null;
                legalEntity.AddressId = address.Id;

                legalEntity.TaxId = _formattingService.FormatTaxId(legalEntity.TaxId);
                legalEntity.Email = string.IsNullOrWhiteSpace(legalEntity.Email) ? null : legalEntity.Email;

                _dbContext.LegalEntities.Add(legalEntity);
                _dbContext.SaveChanges();

                legalEntity.Address = address;

                if (legalEntity.Services == null || !legalEntity.Services.Any())
                    return new TaskResult<int?>(legalEntity.Id);

                legalEntity.LegalEntityServices = new List<LegalEntityService>();
                foreach (var service in legalEntity.Services.Where(service => service != null))
                {
                    legalEntity.LegalEntityServices.Add(new LegalEntityService
                    {
                        LegalEntityId = legalEntity.Id, 
                        ServiceId = service.Id
                    });
                }
                _dbContext.SaveChanges();

                return new TaskResult<int?>(legalEntity.Id);
            }
            catch (Exception e)
            {
                return new TaskResult<int?>(new TaskException(e));
            }
        }
    }
}
