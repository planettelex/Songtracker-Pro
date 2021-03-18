using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.UserTasks
{
    public interface IGetUserTask : ITask<int, User> { }

    public class GetUser : IGetUserTask
    {
        public GetUser(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<User> DoTask(int userId)
        {
            try
            {
                var user = _dbContext.Users.Where(u => u.Id == userId)
                    .Include(u => u.Person).ThenInclude(p => p.Address).ThenInclude(a => a.Country)
                    .Include(u => u.Publisher).ThenInclude(p => p.Address).ThenInclude(a => a.Country)
                    .Include(u => u.RecordLabel).ThenInclude(p => p.Address).ThenInclude(a => a.Country)
                    .Include(p => p.PerformingRightsOrganization).ThenInclude(a => a.Country)
                    .SingleOrDefault();

                if (user?.Publisher != null)
                    user.Publisher.PerformingRightsOrganization = _dbContext.PerformingRightsOrganizations
                        .Where(p => p.Id == user.Publisher.PerformingRightsOrganizationId)
                        .Include(p => p.Country)
                        .SingleOrDefault();

                return new TaskResult<User>(user);
            }
            catch (Exception e)
            {
                return new TaskResult<User>(new TaskException(e));
            }
        }
    }
}
