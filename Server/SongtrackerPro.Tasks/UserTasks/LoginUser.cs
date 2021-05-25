using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.UserTasks
{
    public interface ILoginUserTask : ITask<Login, Login> { }

    public class LoginUser : TaskBase, ILoginUserTask
    {
        public LoginUser(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<Login> DoTask(Login login)
        {
            try
            {
                var existingLogin = _dbContext.Logins.Where(l => l.AuthenticationToken == login.AuthenticationToken)
                    .Include(l => l.User)
                    .SingleOrDefault();

                if (existingLogin != null)
                    return new TaskResult<Login>(existingLogin);
                
                var user = _dbContext.Users.Where(u => u.AuthenticationId == login.AuthenticationId)
                    .Include(u => u.Person).ThenInclude(p => p.Address).ThenInclude(a => a.Country)
                    .Include(u => u.Publisher).ThenInclude(p => p.Address).ThenInclude(a => a.Country)
                    .Include(u => u.Publisher).ThenInclude(p => p.PerformingRightsOrganization).ThenInclude(r => r.Country)
                    .Include(u => u.RecordLabel).ThenInclude(p => p.Address).ThenInclude(a => a.Country)
                    .Include(u => u.PerformingRightsOrganization).ThenInclude(r => r.Country)
                    .SingleOrDefault();

                if (user == null)
                    throw new TaskException(SystemMessage("USER_NOT_FOUND"));

                login.User = null;
                login.UserId = user.Id;
                login.LoginAt = DateTime.UtcNow;

                _dbContext.Logins.Add(login);
                _dbContext.SaveChanges();

                login.User = user;

                return new TaskResult<Login>(login);
            }
            catch (Exception e)
            {
                return new TaskResult<Login>(new TaskException(e));
            }
        }
    }
}
