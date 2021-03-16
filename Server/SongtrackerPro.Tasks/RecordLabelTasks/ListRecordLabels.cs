using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.RecordLabelTasks
{
    public interface IListRecordLabelsTask : ITask<Nothing, List<RecordLabel>> { }

    public class ListRecordLabels : IListRecordLabelsTask
    {
        public ListRecordLabels(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<List<RecordLabel>> DoTask(Nothing nothing)
        {
            try
            {
                var recordLabels = _dbContext.RecordLabels.Include(p => p.Address).ThenInclude(a => a.Country).ToList();

                return new TaskResult<List<RecordLabel>>(recordLabels);
            }
            catch (Exception e)
            {
                return new TaskResult<List<RecordLabel>>(new TaskException(e));
            }
        }
    }
}