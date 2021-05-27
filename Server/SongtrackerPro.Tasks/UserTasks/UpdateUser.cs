using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Tasks.PersonTasks;

namespace SongtrackerPro.Tasks.UserTasks
{
    public interface IUpdateUserTask : ITask<User, Nothing> { }

    public class UpdateUser : TaskBase, IUpdateUserTask
    {
        public UpdateUser(ApplicationDbContext dbContext, IUpdatePersonTask updatePersonTask, IAddPersonTask addPersonTask)
        {
            _dbContext = dbContext;
            _updatePersonTask = updatePersonTask;
            _addPersonTask = addPersonTask;
        }
        private readonly ApplicationDbContext _dbContext;
        private readonly IUpdatePersonTask _updatePersonTask;
        private readonly IAddPersonTask _addPersonTask;

        public TaskResult<Nothing> DoTask(User update)
        {
            try
            {
                var user = _dbContext.Users.Where(u => u.Id == update.Id)
                    .Include(u => u.Person)
                    .SingleOrDefault();

                if (user == null)
                    throw new TaskException(SystemMessage("USER_NOT_FOUND"));

                if (update.Person != null)
                {
                    update.Person.Email = update.AuthenticationId;
                    if (user.Person == null)
                        _addPersonTask.DoTask(update.Person);
                    else
                    {
                        update.Person.Id = user.Person.Id;
                        _updatePersonTask.DoTask(update.Person);
                    }
                }
                
                user.PerformingRightsOrganizationId = update.PerformingRightsOrganization?.Id;
                if (user.PerformingRightsOrganizationId.HasValue)
                {
                    var pro = _dbContext.PerformingRightsOrganizations.SingleOrDefault(r => r.Id == user.PerformingRightsOrganizationId);
                    user.PerformingRightsOrganization = pro ?? throw new TaskException(SystemMessage("PRO_NOT_FOUND"));
                }
                user.PerformingRightsOrganizationMemberNumber = update.PerformingRightsOrganizationMemberNumber;

                user.PublisherId = update.Publisher?.Id;
                if (user.PublisherId.HasValue)
                {
                    var publisher = _dbContext.Publishers.SingleOrDefault(p => p.Id == user.PublisherId);
                    user.Publisher = publisher ?? throw new TaskException(SystemMessage("PUBLISHER_NOT_FOUND"));
                }

                user.RecordLabelId = update.RecordLabel?.Id;
                if (user.RecordLabelId.HasValue)
                {
                    var recordLabel = _dbContext.RecordLabels.SingleOrDefault(p => p.Id == user.PublisherId);
                    user.RecordLabel = recordLabel ?? throw new TaskException(SystemMessage("RECORD_LABEL_NOT_FOUND"));
                }

                user.Type = update.Type;
                user.Roles = update.Roles;
                user.SoundExchangeAccountNumber = update.SoundExchangeAccountNumber;
                user.SocialSecurityNumber = update.SocialSecurityNumber;
                user.AuthenticationId = update.AuthenticationId;

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
