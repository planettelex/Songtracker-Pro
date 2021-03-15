using System;
using System.Collections.Generic;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.PlatformTasks
{
    public interface IListServicesTask : ITask<Nothing, List<Service>> { }

    public class ListServices : IListServicesTask
    {
        public ListServices(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<List<Service>> DoTask(Nothing nothing)
        {
            try
            {
                var services = _dbContext.Services.ToList();

                return new TaskResult<List<Service>>(services);
            }
            catch (Exception e)
            {
                return new TaskResult<List<Service>>(new TaskException(e));
            }
        }
    }
}