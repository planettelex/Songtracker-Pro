using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.RecordLabelTasks
{
    public interface IListRecordingsTask : ITask<RecordLabel, List<Recording>> { }

    public class ListRecordings : TaskBase, IListRecordingsTask
    {
        public ListRecordings(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<List<Recording>> DoTask(RecordLabel recordLabel)
        {
            try
            {
                var recordings = _dbContext.Recordings.Where(r => r.RecordLabelId == recordLabel.Id)
                    .Include(r => r.Artist)
                    .Include(r => r.Genre)
                    .ToList();

                return new TaskResult<List<Recording>>(recordings);
            }
            catch (Exception e)
            {
                return new TaskResult<List<Recording>>(new TaskException(e));
            }
        }
    }
}
