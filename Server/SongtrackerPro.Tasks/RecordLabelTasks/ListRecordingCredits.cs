using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.RecordLabelTasks
{
    public interface IListRecordingCreditsTask : ITask<Nothing, List<RecordingCredit>> { }

    public class ListRecordingCredits : TaskBase, IListRecordingCreditsTask
    {
        public ListRecordingCredits(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<List<RecordingCredit>> DoTask(Nothing nothing)
        {
            try
            {
                var recordingCredits = _dbContext.RecordingCredits
                    .Include(rc => rc.RecordingCreditRoles).ThenInclude(rcr => rcr.RecordingRole)
                    .ToList();

                return new TaskResult<List<RecordingCredit>>(recordingCredits);
            }
            catch (Exception e)
            {
                return new TaskResult<List<RecordingCredit>>(new TaskException(e));
            }
        }
    }
}
