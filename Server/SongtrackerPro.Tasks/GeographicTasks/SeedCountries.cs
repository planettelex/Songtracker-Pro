using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.GeographicTasks
{
    public interface ISeedCountriesTask : ITask<Nothing, bool> { }

    public class SeedCountries : ISeedCountriesTask
    {
        public SeedCountries(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<bool> DoTask(Nothing nothing)
        {
            try
            {
                var countries = _dbContext.Countries.ToList();
                if (countries.Any())
                    return new TaskResult<bool>(false);

                _dbContext.Countries.Add(new Country { Name = "United States", IsoCode = "USA" });
                _dbContext.Countries.Add(new Country { Name = "Canada", IsoCode = "CAN" });

                _dbContext.SaveChanges();

                return new TaskResult<bool>(true);
            }
            catch (Exception e)
            {
                return new TaskResult<bool>(new TaskException(e));
            }
        }
    }
}