using System;
using System.Linq;
using SongtrackerPro.Data;

namespace SongtrackerPro.Tasks.UserTasks
{
    public interface IRemoveUserInvitationTask : ITask<Guid, bool> { }
    public class RemoveUserInvitation : TaskBase, IRemoveUserInvitationTask
    {
        public RemoveUserInvitation(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<bool> DoTask(Guid userInvitationUuid)
        {
            try
            {
                var toRemove = _dbContext.UserInvitations.SingleOrDefault(i => i.Uuid == userInvitationUuid);
                if (toRemove == null)
                    return new TaskResult<bool>(false);

                if (toRemove.AcceptedOn.HasValue || toRemove.CreatedUser != null)
                    throw new ApplicationException(SystemMessage("USER_INVITATION_CANNOT_DELETE_ACCEPTED"));

                _dbContext.UserInvitations.Remove(toRemove);
                _dbContext.SaveChanges();

                return new TaskResult<bool>(true);
            }
            catch (Exception e)
            {
                return new TaskResult<bool>(new TaskException(e));
            }
        }
    }
}
