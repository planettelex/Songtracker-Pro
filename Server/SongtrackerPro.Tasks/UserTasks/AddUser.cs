using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;

namespace SongtrackerPro.Tasks.UserTasks
{
    public interface IAddUserTask : ITask<User, int?> { }

    public class AddUser : TaskBase, IAddUserTask
    {
        public AddUser(ApplicationDbContext dbContext, IFormattingService formattingService)
        {
            _dbContext = dbContext;
            _formattingService = formattingService;
        }
        private readonly ApplicationDbContext _dbContext;
        private readonly IFormattingService _formattingService;

        public TaskResult<int?> DoTask(User user)
        {
            try
            {
                var address = user.Address;
                if (address != null && !string.IsNullOrWhiteSpace(address.Street))
                {
                    var countryId = address.Country?.Id ?? address.CountryId;
                    var country = _dbContext.Countries.SingleOrDefault(c => c.Id == countryId);
                    address.Country = country;
                    _dbContext.Addresses.Add(address);
                    _dbContext.SaveChanges();

                    user.AddressId = address.Id;
                }

                var proId = user.PerformingRightsOrganization?.Id ?? user.PerformingRightsOrganizationId;
                var publisherId = user.Publisher?.Id ?? user.PublisherId;
                var recordLabelId = user.RecordLabel?.Id ?? user.RecordLabelId;

                user.Address = null;
                user.PerformingRightsOrganization = null;
                user.PerformingRightsOrganizationId = proId;
                user.Publisher = null;
                user.PublisherId = publisherId;
                user.RecordLabel = null;
                user.RecordLabelId = recordLabelId;
                user.TaxId = _formattingService.FormatSocialSecurityNumber(user.TaxId);
                user.PerformingRightsOrganizationMemberNumber = string.IsNullOrWhiteSpace(user.PerformingRightsOrganizationMemberNumber) ? null : user.PerformingRightsOrganizationMemberNumber;
                user.SoundExchangeAccountNumber = string.IsNullOrWhiteSpace(user.SoundExchangeAccountNumber) ? null : user.SoundExchangeAccountNumber;
                
                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();

                user.PerformingRightsOrganization = proId > 0 ?
                    _dbContext.PerformingRightsOrganizations.Where(p => p.Id == proId)
                    .Include(p => p.Country)
                    .SingleOrDefault() : null;
                user.Publisher = publisherId > 0 ?
                    _dbContext.Publishers.Where(p => p.Id == publisherId)
                    .Include(p => p.Address).ThenInclude(a => a.Country)
                    .SingleOrDefault() : null;
                user.RecordLabel = recordLabelId > 0 ? 
                    _dbContext.RecordLabels.Where(l => l.Id == recordLabelId)
                    .Include(p => p.Address).ThenInclude(a => a.Country)
                    .SingleOrDefault() : null;

                return new TaskResult<int?>(user.Id);
            }
            catch (Exception e)
            {
                return new TaskResult<int?>(new TaskException(e));
            }
        }
    }
}
