using SongtrackerPro.Data.Models;
using SongtrackerPro.Tasks.GeographicTasks;
using SongtrackerPro.Tasks.PlatformTasks;
using SongtrackerPro.Tasks.PublishingTasks;

namespace SongtrackerPro.Tasks.InstallationTasks
{
    public interface ISeedSystemDataTask : ITask<Nothing, bool> { }

    public class SeedSystemData : ISeedSystemDataTask
    {
        public SeedSystemData(ISeedInstallationTask seedInstallationTask,
                              ISeedCountriesTask seedCountriesTask,
                              ISeedPerformingRightsOrganizationsTask seedPerformingRightsOrganizationsTask,
                              ISeedServicesTask seedServicesTask,
                              ISeedPlatformsTask seedPlatformsTask)
        {
            _seedInstallationTask = seedInstallationTask;
            _seedCountriesTask = seedCountriesTask;
            _seedPerformingRightsOrganizationsTask = seedPerformingRightsOrganizationsTask;
            _seedServicesTask = seedServicesTask;
            _seedPlatformsTask = seedPlatformsTask;
        }
        private readonly ISeedInstallationTask _seedInstallationTask;
        private readonly ISeedCountriesTask _seedCountriesTask;
        private readonly ISeedPerformingRightsOrganizationsTask _seedPerformingRightsOrganizationsTask;
        private readonly ISeedServicesTask _seedServicesTask;
        private readonly ISeedPlatformsTask _seedPlatformsTask;

        public TaskResult<bool> DoTask(Nothing nothing)
        {
            var installationSeeded = _seedInstallationTask.DoTask(nothing).Data;
            var countriesSeeded = _seedCountriesTask.DoTask(nothing).Data;
            var prosSeeded = _seedPerformingRightsOrganizationsTask.DoTask(nothing).Data;
            var servicesSeeded = _seedServicesTask.DoTask(nothing).Data;
            var platformsSeeded = _seedPlatformsTask.DoTask(nothing).Data;

            return new TaskResult<bool>(installationSeeded || countriesSeeded || prosSeeded || servicesSeeded || platformsSeeded);
        }
    }
}
