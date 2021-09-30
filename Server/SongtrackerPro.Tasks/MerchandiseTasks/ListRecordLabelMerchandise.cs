using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.MerchandiseTasks
{
    public interface IListRecordLabelMerchandiseTask : ITask<RecordLabel, List<RecordLabelMerchandiseItem>> { }

    public class ListRecordLabelMerchandise : TaskBase, IListRecordLabelMerchandiseTask
    {
        public ListRecordLabelMerchandise(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<List<RecordLabelMerchandiseItem>> DoTask(RecordLabel recordLabel)
        {
            try
            {
                var merchandiseItems = _dbContext.RecordLabelMerchandise.Where(mi => mi.RecordLabelId == recordLabel.Id)
                    .Include(mi => mi.Artist)
                    .Include(mi => mi.Category)
                    .Include(mi => mi.RecordLabel)
                    .ToList();

                return new TaskResult<List<RecordLabelMerchandiseItem>>(merchandiseItems);
            }
            catch (Exception e)
            {
                return new TaskResult<List<RecordLabelMerchandiseItem>>(new TaskException(e));
            }
        }
    }
}
