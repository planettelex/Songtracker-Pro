using System;
using System.Collections.Generic;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.GeographicTasks
{
    public interface IListCountriesTask : ITask<Nothing, List<Country>> { }

    public class ListCountries : TaskBase, IListCountriesTask
    {
        public ListCountries(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<List<Country>> DoTask(Nothing nothing)
        {
            try
            {
                var countries = _dbContext.Countries.ToList();

                return new TaskResult<List<Country>>(countries);
            }
            catch (Exception e)
            {
                return new TaskResult<List<Country>>(new TaskException(e));
            }
        }
    }
}