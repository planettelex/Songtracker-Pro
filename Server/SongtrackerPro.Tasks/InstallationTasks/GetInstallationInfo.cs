using System;
using System.Linq;
using Microsoft.Data.SqlClient;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Utilities;

namespace SongtrackerPro.Tasks.InstallationTasks
{
    public interface IGetInstallationInfoTask : ITask<Nothing, Installation> { }

    public class GetInstallationInfo : IGetInstallationInfoTask
    {
        public GetInstallationInfo(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<Installation> DoTask(Nothing nothing)
        {
            try
            {
                var installation = _dbContext.Installation.Single();
                var connectionStringBuilder = new SqlConnectionStringBuilder(ApplicationSettings.ConnectionString);
                installation.DatabaseName = connectionStringBuilder.InitialCatalog;

                return new TaskResult<Installation>(installation);
            }
            catch (Exception e)
            {
                return new TaskResult<Installation>(new TaskException(e));
            }
        }
    }
}