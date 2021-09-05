using System;
using System.Collections.Generic;
using System.Linq;
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
                    .Include(i => i.InvitedByUser).ThenInclude(u => u.Person)
                    .Include(i => i.CreatedUser).ThenInclude(u => u.Person)
                    .Include(i => i.Publisher)
                    .Include(i => i.RecordLabel)
                    .Include(i => i.Artist)
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
