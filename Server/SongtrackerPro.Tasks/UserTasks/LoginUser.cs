using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Enums;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Resources;
using SongtrackerPro.Tasks.InstallationTasks;
using SongtrackerPro.Utilities;

namespace SongtrackerPro.Tasks.UserTasks
{
    public interface ILoginUserTask : ITask<Login, Login> { }

    public class LoginUser : TaskBase, ILoginUserTask
    {
        public LoginUser(ApplicationDbContext dbContext,
                         IGetInstallationTask getInstallationTask,
                         ISeedSystemDataTask seedSystemDataTask)
        {
            _dbContext = dbContext;
            _getInstallationTask = getInstallationTask;
            _seedSystemDataTask = seedSystemDataTask;

        }
        private readonly ApplicationDbContext _dbContext;
        private readonly IGetInstallationTask _getInstallationTask;
        private readonly ISeedSystemDataTask _seedSystemDataTask;

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
                    .Include(u => u.Address).ThenInclude(a => a.Country)
                    .Include(u => u.Publisher).ThenInclude(p => p.Address).ThenInclude(a => a.Country)
                    .Include(u => u.Publisher).ThenInclude(p => p.PerformingRightsOrganization).ThenInclude(r => r.Country)
                    .Include(u => u.RecordLabel).ThenInclude(p => p.Address).ThenInclude(a => a.Country)
                    .Include(u => u.PerformingRightsOrganization).ThenInclude(r => r.Country)
                    .SingleOrDefault();

                if (user == null)
                {
                    if (login.AuthenticationId == ApplicationSettings.Mail.From)
                    {
                        user = new User
                        {
                            AuthenticationId = login.AuthenticationId, 
                            UserType = UserType.SystemAdministrator,
                            Name = GetResource.SystemMessage(ApplicationSettings.Culture, "SUPERUSER")
                        };
                        _dbContext.Users.Add(user);
                        _dbContext.SaveChanges();

                        var installation = _getInstallationTask.DoTask(null);
                        if (installation.HasNoData)
                            _seedSystemDataTask.DoTask(null);
                    }
                    else
                        throw new TaskException(SystemMessage("USER_NOT_FOUND"));
                }

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
