using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.UserTasks
{
    public interface IAddUserTask : ITask<User, int?> { }

    public class AddUser : IAddUserTask
    {
        public AddUser(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<int?> DoTask(User user)
        {
            try
            {
                var person = user.Person;
                //var personId =



                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();

                //user.RecordLabel = recordLabel;

                return new TaskResult<int?>(user.Id);
            }
            catch (Exception e)
            {
                return new TaskResult<int?>(new TaskException(e));
            }
        }
    }
}
