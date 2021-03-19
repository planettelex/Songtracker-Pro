using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.UserTasks
{
    public interface IGetUserInvitationTask : ITask<Guid, UserInvitation> { }

    public class GetUserInvitation : TaskBase, IGetUserInvitationTask
    {
        public GetUserInvitation(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<UserInvitation> DoTask(Guid userInvitationId)
        {
            try
            {
                var userInvitation = _dbContext.UserInvitations.Where(ui => ui.Uuid == userInvitationId)
                    .Include(u => u.Publisher).ThenInclude(p => p.Address).ThenInclude(a => a.Country)
                    .Include(u => u.RecordLabel).ThenInclude(p => p.Address).ThenInclude(a => a.Country)
                    .Include(p => p.Artist)
                    .SingleOrDefault();

                return new TaskResult<UserInvitation>(userInvitation);
            }
            catch (Exception e)
            {
                return new TaskResult<UserInvitation>(new TaskException(e));
            }
        }
    }
}
