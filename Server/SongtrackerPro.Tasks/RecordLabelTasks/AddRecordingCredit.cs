using System;
using System.Collections.Generic;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.RecordLabelTasks
{
    public interface IAddRecordingCreditTask : ITask<RecordingCredit, int?> { }

    public class AddRecordingCredit : TaskBase, IAddRecordingCreditTask
    {
        public AddRecordingCredit(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<int?> DoTask(RecordingCredit recordingCredit)
        {
            try
            {
                _dbContext.RecordingCredits.Add(recordingCredit);
                _dbContext.SaveChanges();

                if (recordingCredit.Roles == null || !recordingCredit.Roles.Any())
                    return new TaskResult<int?>(recordingCredit.Id);

                recordingCredit.RecordingCreditRoles = new List<RecordingCreditRole>();
                foreach (var recordingRole in recordingCredit.Roles.Where(role => role != null))
                {
                    recordingCredit.RecordingCreditRoles.Add(new RecordingCreditRole
                    {
                        RecordingCreditId = recordingCredit.Id,
                        RecordingRoleId = recordingRole.Id
                    });
                }
                _dbContext.SaveChanges();

                return new TaskResult<int?>(recordingCredit.Id);
            }
            catch (Exception e)
            {
                return new TaskResult<int?>(new TaskException(e));
            }
        }
    }
}
