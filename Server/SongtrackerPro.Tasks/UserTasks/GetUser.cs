using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.UserTasks
{
    public interface IGetUserTask : ITask<int, User> { }

    public class GetUser : TaskBase, IGetUserTask
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
                    .Include(u => u.Address).ThenInclude(a => a.Country)
                    .Include(u => u.Publisher).ThenInclude(p => p.Address).ThenInclude(a => a.Country)
                    .Include(u => u.Publisher).ThenInclude(p => p.PerformingRightsOrganization).ThenInclude(r => r.Country)
                    .Include(u => u.RecordLabel).ThenInclude(p => p.Address).ThenInclude(a => a.Country)
                    .Include(u => u.PerformingRightsOrganization).ThenInclude(r => r.Country)
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
