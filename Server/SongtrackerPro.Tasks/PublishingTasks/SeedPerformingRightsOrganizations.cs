using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Tasks.GeographicTasks;

namespace SongtrackerPro.Tasks.PublishingTasks
{
    public interface ISeedPerformingRightsOrganizationsTask : ITask<Nothing, bool> { }

    public class SeedPerformingRightsOrganizations : TaskBase, ISeedPerformingRightsOrganizationsTask
    {
        public SeedPerformingRightsOrganizations(ApplicationDbContext dbContext, ISeedCountriesTask seedCountriesTask)
        {
            _dbContext = dbContext;
            _seedCountriesTask = seedCountriesTask;
        }
        private readonly ApplicationDbContext _dbContext;
        private readonly ISeedCountriesTask _seedCountriesTask;

        public TaskResult<bool> DoTask(Nothing nothing)
        {
            try
            {
                var pros = _dbContext.PerformingRightsOrganizations.ToList();
                if (pros.Any())
                    return new TaskResult<bool>(false);

                _seedCountriesTask.DoTask(nothing);

                var us = _dbContext.Countries.SingleOrDefault(c => c.IsoCode.ToUpper() == "US");
                if (us == null)
                    throw new NullReferenceException(SystemMessage("USA_NOT_FOUND"));

                _dbContext.PerformingRightsOrganizations.Add(new PerformingRightsOrganization { Name = "ASCAP", CountryId = us.Id });
                _dbContext.PerformingRightsOrganizations.Add(new PerformingRightsOrganization { Name = "BMI", CountryId = us.Id });

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