using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.RecordLabelTasks
{
    public interface IGetRecordLabelTask : ITask<int, RecordLabel> { }

    public class GetRecordLabel : IGetRecordLabelTask
    {
        public GetRecordLabel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<RecordLabel> DoTask(int recordLabelId)
        {
            try
            {
                var recordLabel = _dbContext.RecordLabels.Where(p => p.Id == recordLabelId)
                    .Include(p => p.Address).ThenInclude(a => a.Country)
                    .SingleOrDefault();

                return new TaskResult<RecordLabel>(recordLabel);
            }
            catch (Exception e)
            {
                return new TaskResult<RecordLabel>(new TaskException(e));
            }
        }
    }
}
