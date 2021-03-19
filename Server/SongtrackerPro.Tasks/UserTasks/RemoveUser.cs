using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.UserTasks
{
    public interface IRemoveUserTask : ITask<User, bool> { }

    public class RemoveUser : TaskBase, IRemoveUserTask
    {
        public RemoveUser(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<bool> DoTask(User user)
        {
            try
            {
                var toRemove = _dbContext.Users.SingleOrDefault(u => u.Id == user.Id);
                if (toRemove == null)
                    return new TaskResult<bool>(false);

                toRemove.Person = null;
                toRemove.PersonId = null;
                toRemove.PerformingRightsOrganization = null;
                toRemove.PerformingRightsOrganizationId = null;
                toRemove.Publisher = null;
                toRemove.PublisherId = null;
                toRemove.RecordLabel = null;
                toRemove.RecordLabelId = null;
                _dbContext.SaveChanges();

                _dbContext.Users.Remove(toRemove);
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
