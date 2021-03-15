using SongtrackerPro.Data.Models;
using SongtrackerPro.Tasks.GeographicTasks;
using SongtrackerPro.Tasks.PublishingTasks;

namespace SongtrackerPro.Tasks.InstallationTasks
{
    public interface ISeedSystemDataTask : ITask<Nothing, bool> { }

    public class SeedSystemData : ISeedSystemDataTask
    {
        public SeedSystemData(ISeedInstallationTask seedInstallationTask,
                              ISeedCountriesTask seedCountriesTask,
                              ISeedPerformingRightsOrganizationsTask seedPerformingRightsOrganizationsTask)
        {
            _seedInstallationTask = seedInstallationTask;
            _seedCountriesTask = seedCountriesTask;
            _seedPerformingRightsOrganizationsTask = seedPerformingRightsOrganizationsTask;
        }
        private readonly ISeedInstallationTask _seedInstallationTask;
        private readonly ISeedCountriesTask _seedCountriesTask;
        private readonly ISeedPerformingRightsOrganizationsTask _seedPerformingRightsOrganizationsTask;

        public TaskResult<bool> DoTask(Nothing nothing)
        {
            var installationSeeded = _seedInstallationTask.DoTask(nothing).Data;
            var countriesSeeded = _seedCountriesTask.DoTask(nothing).Data;
            var prosSeeded = _seedPerformingRightsOrganizationsTask.DoTask(nothing).Data;

            return new TaskResult<bool>(installationSeeded || countriesSeeded || prosSeeded);
        }
    }
}
