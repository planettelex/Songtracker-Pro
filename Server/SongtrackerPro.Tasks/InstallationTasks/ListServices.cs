using System;
using System.Collections.Generic;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Enums;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.InstallationTasks
{
    public interface IListServicesTask : ITask<ServiceType, List<Service>> { }

    public class ListServices : TaskBase, IListServicesTask
    {
        public ListServices(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<List<Service>> DoTask(ServiceType serviceType)
        {
            try
            {
                List<Service> services = null;
                switch (serviceType)
                {
                    case ServiceType.Unspecified:
                        services = _dbContext.Services.ToList();
                        break;
                    case ServiceType.Platform:
                        services = _dbContext.Services.Where(s => s.Type == ServiceType.Platform).ToList();
                        break;
                    case ServiceType.LegalEntity:
                        services = _dbContext.Services.Where(s => s.Type == ServiceType.LegalEntity).ToList();
                        break;
                }
                
                return new TaskResult<List<Service>>(services);
            }
            catch (Exception e)
            {
                return new TaskResult<List<Service>>(new TaskException(e));
            }
        }
    }
}