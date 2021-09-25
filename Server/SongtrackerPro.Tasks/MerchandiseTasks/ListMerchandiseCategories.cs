using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.MerchandiseTasks
{
    public interface IListMerchandiseCategoriesTask : ITask<Nothing, List<MerchandiseCategory>> { }

    public class ListMerchandiseCategories : TaskBase, IListMerchandiseCategoriesTask
    {
        public ListMerchandiseCategories(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<List<MerchandiseCategory>> DoTask(Nothing nothing)
        {
            try
            {
                var merchandiseCategories = _dbContext.MerchandiseCategories
                    .Include(mc => mc.ParentCategory).ThenInclude(pc => pc.ParentCategory)
                    .ToList();

                return new TaskResult<List<MerchandiseCategory>>(merchandiseCategories);
            }
            catch (Exception e)
            {
                return new TaskResult<List<MerchandiseCategory>>(new TaskException(e));
            }
        }
    }
}
