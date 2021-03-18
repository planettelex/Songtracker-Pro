using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.UserTasks
{
    public interface ILoginUserTask : ITask<Login, User> { }

    public class LoginUser : ILoginUserTask
    {
        public LoginUser(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<User> DoTask(Login login)
        {
            try
            {
                var user = _dbContext.Users.Where(u => u.AuthenticationId == login.AuthenticationId)
                    .Include(u => u.Person).ThenInclude(p => p.Address).ThenInclude(a => a.Country)
                    .Include(u => u.Publisher).ThenInclude(p => p.Address).ThenInclude(a => a.Country)
                    .Include(u => u.RecordLabel).ThenInclude(p => p.Address).ThenInclude(a => a.Country)
                    .Include(p => p.PerformingRightsOrganization).ThenInclude(a => a.Country)
                    .SingleOrDefault();

                if (user == null)
                    throw new TaskException("User not found.");

                user.AuthenticationToken = login.AuthenticationToken;
                user.LastLogin = DateTime.UtcNow;
                _dbContext.SaveChanges();

                if (user.Publisher != null)
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
