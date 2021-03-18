using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Tasks.PersonTasks;
using SongtrackerPro.Utilities.Services;

namespace SongtrackerPro.Tasks.UserTasks
{
    public interface IAcceptUserInvitationTask : ITask<UserInvitation, User> { }

    public class AcceptUserInvitation : IAcceptUserInvitationTask
    {
        public AcceptUserInvitation(ApplicationDbContext dbContext,
                                    IEmailService emailService, 
                                    IAddPersonTask addPersonTask)
        {
            _dbContext = dbContext;
            _emailService = emailService;
            _addPersonTask = addPersonTask;
        }
        private readonly ApplicationDbContext _dbContext;
        private readonly IEmailService _emailService;
        private readonly IAddPersonTask _addPersonTask;
        
        public TaskResult<User> DoTask(UserInvitation userInvitation)
        {
            try
            {
                var newUser = userInvitation.CreatedUser;
                var newPerson = newUser.Person;
                var addPersonResult = _addPersonTask.DoTask(newPerson);
                if (!addPersonResult.Success)
                    throw addPersonResult.Exception;

                var personId = addPersonResult.Data;
                var proId = newUser.PerformingRightsOrganization?.Id ?? newUser.PerformingRightsOrganizationId;
                var publisherId = userInvitation.Publisher?.Id ?? userInvitation.PublisherId;
                var recordLabelId = userInvitation.RecordLabel?.Id ?? userInvitation.RecordLabelId;
                var artistId = userInvitation.Artist?.Id ?? userInvitation.ArtistId;

                newUser.Person = null;
                newUser.PersonId = personId;
                newUser.PerformingRightsOrganization = null;
                newUser.PerformingRightsOrganizationId = proId;
                newUser.Publisher = null;
                newUser.PublisherId = publisherId;
                newUser.RecordLabel = null;
                newUser.RecordLabelId = recordLabelId;

                _dbContext.Users.Add(newUser);
                _dbContext.SaveChanges();
                //TODO: Add artist member or manager

                newUser.Person = newPerson;
                newUser.PerformingRightsOrganization = proId > 0 ?
                    _dbContext.PerformingRightsOrganizations.Where(p => p.Id == proId)
                        .Include(p => p.Country)
                        .SingleOrDefault() : null;
                newUser.Publisher = publisherId > 0 ?
                    _dbContext.Publishers.Where(p => p.Id == publisherId)
                        .Include(p => p.Address).ThenInclude(a => a.Country)
                        .SingleOrDefault() : null;
                newUser.RecordLabel = recordLabelId > 0 ? 
                    _dbContext.RecordLabels.Where(l => l.Id == recordLabelId)
                        .Include(p => p.Address).ThenInclude(a => a.Country)
                        .SingleOrDefault() : null;

                // TODO: Get title and body from resources and replace tokens.
                _emailService.SendEmail(newUser.Person.Email, "Title", "Body");

                return new TaskResult<User>(newUser);
            }
            catch (Exception e)
            {
                return new TaskResult<User>(new TaskException(e));
            }
        }
    }
}
