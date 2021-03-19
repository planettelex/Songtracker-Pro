using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Tasks.PersonTasks;

namespace SongtrackerPro.Tasks.UserTasks
{
    public interface IUpdateUserTask : ITask<User, Nothing> { }

    public class UpdateUser : TaskBase, IUpdateUserTask
    {
        public UpdateUser(ApplicationDbContext dbContext, IUpdatePersonTask updatePersonTask)
        {
            _dbContext = dbContext;
            _updatePersonTask = updatePersonTask;
        }
        private readonly ApplicationDbContext _dbContext;
        private readonly IUpdatePersonTask _updatePersonTask;

        public TaskResult<Nothing> DoTask(User update)
        {
            try
            {
                var user = _dbContext.Users.SingleOrDefault(u => u.Id == update.Id);

                if (user == null)
                    throw new TaskException(SystemMessage("USER_NOT_FOUND"));

                _updatePersonTask.DoTask(update.Person);

                user.ProfileImageUrl = update.ProfileImageUrl;
                user.SocialSecurityNumber = update.SocialSecurityNumber;
                user.PerformingRightsOrganizationId = update.PerformingRightsOrganization?.Id ?? update.PerformingRightsOrganizationId;
                user.PerformingRightsOrganizationMemberNumber = update.PerformingRightsOrganizationMemberNumber;
                user.SoundExchangeAccountNumber = update.SoundExchangeAccountNumber;
                user.PublisherId = update.Publisher?.Id ?? update.PublisherId;
                user.RecordLabelId = update.RecordLabel?.Id ?? update.RecordLabelId;
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
