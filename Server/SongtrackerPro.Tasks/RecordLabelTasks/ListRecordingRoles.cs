using System;
using System.Collections.Generic;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.RecordLabelTasks
{
    public interface IListRecordingRolesTask : ITask<Nothing, List<RecordingRole>> { }

    public class ListRecordingRoles : TaskBase, IListRecordingRolesTask
    {
        public ListRecordingRoles(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<List<RecordingRole>> DoTask(Nothing nothing)
        {
            try
            {
                var recordingRoles = _dbContext.RecordingRoles.ToList();

                return new TaskResult<List<RecordingRole>>(recordingRoles);
            }
            catch (Exception e)
            {
                return new TaskResult<List<RecordingRole>>(new TaskException(e));
            }
        }
    }
}
