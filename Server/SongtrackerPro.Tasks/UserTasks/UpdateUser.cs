using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;

namespace SongtrackerPro.Tasks.UserTasks
{
    public interface IUpdateUserTask : ITask<User, Nothing> { }

    public class UpdateUser : TaskBase, IUpdateUserTask
    {
        public UpdateUser(ApplicationDbContext dbContext, IFormattingService formattingService)
        {
            _dbContext = dbContext;
            _formattingService = formattingService;
        }
        private readonly ApplicationDbContext _dbContext;
        private readonly IFormattingService _formattingService;

        public TaskResult<Nothing> DoTask(User update)
        {
            try
            {
                var user = _dbContext.Users.Where(u => u.Id == update.Id)
                    .Include(u => u.Address).ThenInclude(a => a.Country)
                    .SingleOrDefault();

                if (user == null)
                    throw new TaskException(SystemMessage("USER_NOT_FOUND"));

                user.FirstName = update.FirstName;
                user.MiddleName = update.MiddleName;
                user.LastName = update.LastName;
                user.NameSuffix = update.NameSuffix;
                user.Name = update.Name ?? update.FirstAndLastName;
                user.Phone = update.Phone;
                user.AuthenticationId = update.AuthenticationId;
                user.Email = update.Email;
                user.UserRoles = update.UserRoles;
                user.PerformingRightsOrganizationMemberNumber = string.IsNullOrWhiteSpace(update.PerformingRightsOrganizationMemberNumber) ? null : update.PerformingRightsOrganizationMemberNumber;
                user.SoundExchangeAccountNumber = string.IsNullOrWhiteSpace(update.SoundExchangeAccountNumber) ? null : update.SoundExchangeAccountNumber;
                user.TaxId = _formattingService.FormatSocialSecurityNumber(update.TaxId);

                user.PerformingRightsOrganizationId = update.PerformingRightsOrganization?.Id;
                if (user.PerformingRightsOrganizationId.HasValue)
                {
                    var pro = _dbContext.PerformingRightsOrganizations.SingleOrDefault(r => r.Id == user.PerformingRightsOrganizationId);
                    user.PerformingRightsOrganization = pro ?? throw new TaskException(SystemMessage("PRO_NOT_FOUND"));
                }

                user.PublisherId = update.Publisher?.Id;
                if (user.PublisherId.HasValue)
                {
                    var publisher = _dbContext.Publishers.SingleOrDefault(p => p.Id == user.PublisherId);
                    user.Publisher = publisher ?? throw new TaskException(SystemMessage("PUBLISHER_NOT_FOUND"));
                }

                user.RecordLabelId = update.RecordLabel?.Id;
                if (user.RecordLabelId.HasValue)
                {
                    var recordLabel = _dbContext.RecordLabels.SingleOrDefault(p => p.Id == user.RecordLabelId);
                    user.RecordLabel = recordLabel ?? throw new TaskException(SystemMessage("RECORD_LABEL_NOT_FOUND"));
                }

                if (update.Address != null && !string.IsNullOrWhiteSpace(update.Address.Street))
                {
                    if (user.Address == null)
                    {
                        var address = update.Address;
                        var countryId = address.Country?.Id ?? address.CountryId;
                        var country = _dbContext.Countries.SingleOrDefault(c => c.Id == countryId);
                        address.Country = country;
                        _dbContext.Addresses.Add(address);
                        _dbContext.SaveChanges();

                        user.Address = address;
                    }
                    user.Address.Street = update.Address.Street;
                    user.Address.City = update.Address.City;
                    user.Address.Region = update.Address.Region;
                    user.Address.PostalCode = update.Address.PostalCode;
                    user.Address.CountryId = update.Address.Country?.Id;
                    if (user.Address.CountryId.HasValue)
                    {
                        var country = _dbContext.Countries.SingleOrDefault(c => c.Id == user.Address.CountryId);
                        user.Address.Country = country ?? throw new TaskException(SystemMessage("COUNTRY_NOT_FOUND"));
                    }
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
