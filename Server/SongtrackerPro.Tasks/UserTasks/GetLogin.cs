using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.UserTasks
{
    public interface IGetLoginTask : ITask<string, Login> { }

    public class GetLogin : TaskBase, IGetLoginTask
    {
        public GetLogin(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<Login> DoTask(string authenticationToken)
        {
            var login = _dbContext.Logins.Where(l => l.AuthenticationToken == authenticationToken)
                .Include(l => l.User).ThenInclude(u => u.Person).ThenInclude(p => p.Address)
                .Include(l => l.User).ThenInclude(u => u.Publisher).ThenInclude(p => p.Address).ThenInclude(a => a.Country)
                .Include(l => l.User).ThenInclude(u => u.Publisher).ThenInclude(p => p.PerformingRightsOrganization).ThenInclude(a => a.Country)
                .Include(l => l.User).ThenInclude(u => u.RecordLabel).ThenInclude(p => p.Address).ThenInclude(a => a.Country)
                .Include(l => l.User).ThenInclude(u => u.PerformingRightsOrganization).ThenInclude(a => a.Country)
                .SingleOrDefault();

            if (login == null)
                throw new TaskException(SystemMessage("LOGIN_NOT_FOUND"));

            return new TaskResult<Login>(login);
        }
    }
}
