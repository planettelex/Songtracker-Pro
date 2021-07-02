using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Enums;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.UserTasks
{
    public interface IListUsersTask : ITask<UserType?, List<User>> { }

    public class ListUsers : TaskBase, IListUsersTask
    {
        public ListUsers(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<List<User>> DoTask(UserType? userType)
        {
            try
            {
                List<User> users;
                if (userType.HasValue) 
                    users = _dbContext.Users.Where(u => u.Type == userType.Value)
                        .Include(u => u.Person).ThenInclude(p => p.Address).ThenInclude(a => a.Country)
                        .Include(u => u.PerformingRightsOrganization).ThenInclude(p => p.Country)
                        .Include(u => u.Publisher).ThenInclude(p => p.PerformingRightsOrganization).ThenInclude(p => p.Country)
                        .OrderBy(u => u.Person.FirstName).ThenBy(u => u.Person.LastName)
                        .ToList();
                else
                    users = _dbContext.Users
                        .Include(u => u.Person).ThenInclude(p => p.Address).ThenInclude(a => a.Country)
                        .Include(u => u.PerformingRightsOrganization).ThenInclude(p => p.Country)
                        .Include(u => u.Publisher).ThenInclude(p => p.PerformingRightsOrganization).ThenInclude(p => p.Country)
                        .OrderBy(u => u.Person.FirstName).ThenBy(u => u.Person.LastName)
                        .ToList();
                
                return new TaskResult<List<User>>(users);
            }
            catch (Exception e)
            {
                return new TaskResult<List<User>>(new TaskException(e));
            }
        }
    }
}