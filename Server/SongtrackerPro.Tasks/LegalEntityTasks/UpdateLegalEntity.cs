using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;

namespace SongtrackerPro.Tasks.LegalEntityTasks
{
    public interface IUpdateLegalEntityTask : ITask<LegalEntity, Nothing> { }

    public class UpdateLegalEntity : TaskBase, IUpdateLegalEntityTask
    {
        public UpdateLegalEntity(ApplicationDbContext dbContext, IFormattingService formattingService)
        {
            _dbContext = dbContext;
            _formattingService = formattingService;
        }
        private readonly ApplicationDbContext _dbContext;
        private readonly IFormattingService _formattingService;

        public TaskResult<Nothing> DoTask(LegalEntity update)
        {
            try
            {
                var legalEntity = _dbContext.LegalEntities.Where(p => p.Id == update.Id)
                    .Include(p => p.LegalEntityServices).ThenInclude(les => les.Service).SingleOrDefault();

                if (legalEntity == null)
                    throw new TaskException(SystemMessage("LEGAL_ENTITY_NOT_FOUND"));

                legalEntity.Name = update.Name;
                legalEntity.TaxId = _formattingService.FormatTaxId(update.TaxId);
                legalEntity.Email = string.IsNullOrWhiteSpace(update.Email) ? null : update.Email;
                legalEntity.EntityType = update.EntityType;
                legalEntity.Phone = update.Phone;
                _dbContext.SaveChanges();

                if (update.Address != null)
                {
                    if (legalEntity.Address == null)
                    {
                        var address = update.Address;
                        var countryId = address.Country?.Id ?? address.CountryId;
                        var country = _dbContext.Countries.SingleOrDefault(c => c.Id == countryId);
                        address.Country = country;
                        _dbContext.Addresses.Add(address);
                        _dbContext.SaveChanges();
                        legalEntity.Address = address;
                    }

                    legalEntity.Address.Street = update.Address.Street;
                    legalEntity.Address.City = update.Address.City;
                    legalEntity.Address.Region = update.Address.Region;
                    legalEntity.Address.PostalCode = update.Address.PostalCode;
                    legalEntity.Address.CountryId = update.Address.Country?.Id;
                    if (legalEntity.Address.CountryId.HasValue)
                    {
                        var country = _dbContext.Countries.SingleOrDefault(c => c.Id == legalEntity.Address.CountryId);
                        legalEntity.Address.Country = country ?? throw new TaskException(SystemMessage("COUNTRY_NOT_FOUND"));
                    }
                }

                if (update.Services == null) 
                    return new TaskResult<Nothing>(null as Nothing);

                foreach (var legalEntityService in legalEntity.LegalEntityServices)
                {
                    var service = update.Services.SingleOrDefault(s => s?.Id == legalEntityService.ServiceId);
                    if (service == null)
                        _dbContext.LegalEntityServices.Remove(legalEntityService);
                }

                _dbContext.SaveChanges();

                foreach (var service in update.Services.Where(service => service != null))
                {
                    var legalEntityService = legalEntity.LegalEntityServices.SingleOrDefault(les => les.ServiceId == service.Id);
                    if (legalEntityService == null)
                        legalEntity.LegalEntityServices.Add(new LegalEntityService { LegalEntityId = legalEntity.Id, ServiceId = service.Id });
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
