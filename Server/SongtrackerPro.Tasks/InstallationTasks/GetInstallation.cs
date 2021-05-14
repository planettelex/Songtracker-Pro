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
                installation.DatabaseServer = connectionStringBuilder.DataSource;
                installation.DatabaseName = connectionStringBuilder.InitialCatalog;
                installation.DatabaseConsole = ApplicationSettings.Database.HostingConsole;
                installation.Domain = ApplicationSettings.Web.Domain;
                installation.HostingConsole = ApplicationSettings.Web.HostingConsole;
                installation.OAuthId = ApplicationSettings.Web.OAuthId;
                installation.OAuthConsole = ApplicationSettings.Web.OAuthConsole;
                installation.ApiDomain = ApplicationSettings.Api.Domain;
                installation.ApiHostingConsole = ApplicationSettings.Api.HostingConsole;
                installation.EmailServer = ApplicationSettings.Mail.Smtp;
                installation.EmailAccount = ApplicationSettings.Mail.From;
                installation.EmailConsole = ApplicationSettings.Mail.WebConsole;
                installation.Culture = ApplicationSettings.Culture;
                installation.Currency = ApplicationSettings.Currency;

                return new TaskResult<Installation>(installation);
            }
            catch (Exception e)
            {
                return new TaskResult<Installation>(new TaskException(e));
            }
        }
    }
}