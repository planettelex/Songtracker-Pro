using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.PersonTasks;

namespace SongtrackerPro.Tasks.UserTasks
{
    public interface IAddUserTask : ITask<User, int?> { }

    public class AddUser : TaskBase, IAddUserTask
    {
        public AddUser(ApplicationDbContext dbContext, IAddPersonTask addPersonTask, IFormattingService formattingService)
        {
            _dbContext = dbContext;
            _addPersonTask = addPersonTask;
            _formattingService = formattingService;
        }
        private readonly ApplicationDbContext _dbContext;
        private readonly IAddPersonTask _addPersonTask;
        private readonly IFormattingService _formattingService;

        public TaskResult<int?> DoTask(User user)
        {
            try
            {
                var proId = user.PerformingRightsOrganization?.Id ?? user.PerformingRightsOrganizationId;
                var publisherId = user.Publisher?.Id ?? user.PublisherId;
                var recordLabelId = user.RecordLabel?.Id ?? user.RecordLabelId;

                user.PerformingRightsOrganization = null;
                user.PerformingRightsOrganizationId = proId;
                user.Publisher = null;
                user.PublisherId = publisherId;
                user.RecordLabel = null;
                user.RecordLabelId = recordLabelId;

                var person = user.Person;
                if (person != null)
                {
                    person.Email = user.AuthenticationId;
                    var addPersonResult = _addPersonTask.DoTask(person);
                    if (!addPersonResult.Success)
                        throw addPersonResult.Exception;

                    var personId = addPersonResult.Data;
                    user.Person = null;
                    user.PersonId = personId;
                }

                user.SocialSecurityNumber = _formattingService.FormatSocialSecurityNumber(user.SocialSecurityNumber);
                user.PerformingRightsOrganizationMemberNumber = string.IsNullOrWhiteSpace(user.PerformingRightsOrganizationMemberNumber) ? null : user.PerformingRightsOrganizationMemberNumber;
                user.SoundExchangeAccountNumber = string.IsNullOrWhiteSpace(user.SoundExchangeAccountNumber) ? null : user.SoundExchangeAccountNumber;
                
                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();

                user.Person = person;
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
