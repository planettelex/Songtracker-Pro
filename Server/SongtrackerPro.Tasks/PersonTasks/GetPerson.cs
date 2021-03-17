using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.PersonTasks
{
    public interface IGetPersonTask : ITask<int, Person> { }

    public class GetPerson : IGetPersonTask
    {
        public GetPerson(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<Person> DoTask(int personId)
        {
            try
            {
                var person = _dbContext.People.Where(p => p.Id == personId)
                    .Include(p => p.Address).ThenInclude(a => a.Country)
                    .SingleOrDefault();

                return new TaskResult<Person>(person);
            }
            catch (Exception e)
            {
                return new TaskResult<Person>(new TaskException(e));
            }
        }
    }
}
