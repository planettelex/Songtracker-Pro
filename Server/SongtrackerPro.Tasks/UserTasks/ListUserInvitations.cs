using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.UserTasks
{
    public interface IListUserInvitationsTask : ITask<Nothing, List<UserInvitation>> { }

    public class ListUserInvitations : TaskBase, IListUserInvitationsTask
    {
        public ListUserInvitations(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<List<UserInvitation>> DoTask(Nothing input)
        {
            try
            {
                var userInvitations = _dbContext.UserInvitations
                    .Include(u => u.InvitedByUser).ThenInclude(u => u.Person).ThenInclude(p => p.Address).ThenInclude(a => a.Country)
                    .Include(u => u.CreatedUser).ThenInclude(u => u.Person).ThenInclude(p => p.Address).ThenInclude(a => a.Country)
                    .ToList();

                return new TaskResult<List<UserInvitation>>(userInvitations);
            }
            catch (Exception e)
            {
                return new TaskResult<List<UserInvitation>>(new TaskException(e));
            }
        }
    }
}
