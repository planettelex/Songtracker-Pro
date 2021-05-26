using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.GeographicTasks
{
    public interface ISeedCountriesTask : ITask<Nothing, bool> { }

    public class SeedCountries : TaskBase, ISeedCountriesTask
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

                _dbContext.Countries.Add(new Country { Name = SeedData("COUNTRY_UNITED_STATES"), IsoCode = "US" });
                _dbContext.Countries.Add(new Country { Name = SeedData("COUNTRY_CANADA"), IsoCode = "CA" });

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