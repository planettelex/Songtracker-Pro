using System;
using System.Linq;
using Microsoft.Data.SqlClient;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Utilities;

namespace SongtrackerPro.Tasks.InstallationTasks
{
    public interface IGetInstallationTask : ITask<Nothing, Installation> { }

    public class GetInstallation : TaskBase, IGetInstallationTask
    {
        public GetInstallation(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<Installation> DoTask(Nothing nothing)
        {
            try
            {
                var installation = _dbContext.Installation.SingleOrDefault();
                if (installation == null)
                    return new TaskResult<Installation>(null as Installation);

                var connectionStringBuilder = new SqlConnectionStringBuilder(ApplicationSettings.Database.ConnectionString);
                installation.DatabaseName = connectionStringBuilder.InitialCatalog;
                installation.DatabaseConsole = ApplicationSettings.Database.HostingConsole;
                installation.OAuthId = ApplicationSettings.Web.OAuthId;
                installation.OAuthConsole = ApplicationSettings.Web.OAuthConsole;
                installation.ApiHostingConsole = ApplicationSettings.Api.HostingConsole;

                return new TaskResult<Installation>(installation);
            }
            catch (Exception e)
            {
                return new TaskResult<Installation>(new TaskException(e));
            }
        }
    }
}