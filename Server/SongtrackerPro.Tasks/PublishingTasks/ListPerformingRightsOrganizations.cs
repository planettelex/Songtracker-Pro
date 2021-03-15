using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.PublishingTasks
{
    public interface IListPerformingRightsOrganizationsTask : ITask<Nothing, List<PerformingRightsOrganization>> { }

    public class ListPerformingRightsOrganizations : IListPerformingRightsOrganizationsTask
    {
        public ListPerformingRightsOrganizations(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<List<PerformingRightsOrganization>> DoTask(Nothing nothing)
        {
            try
            {
                var performingRightsOrganizations = _dbContext.PerformingRightsOrganizations.Include(pro => pro.Country).ToList();

                return new TaskResult<List<PerformingRightsOrganization>>(performingRightsOrganizations);
            }
            catch (Exception e)
            {
                return new TaskResult<List<PerformingRightsOrganization>>(new TaskException(e));
            }
        }
    }
}