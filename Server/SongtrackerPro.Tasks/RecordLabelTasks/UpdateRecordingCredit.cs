using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.RecordLabelTasks
{
    public interface IUpdateRecordingCreditTask : ITask<RecordingCredit, Nothing> { }

    public class UpdateRecordingCredit : TaskBase, IUpdateRecordingCreditTask
    {
        public UpdateRecordingCredit(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<Nothing> DoTask(RecordingCredit update)
        {
            try
            {
                var recordingCredit = _dbContext.RecordingCredits.Where(rc => rc.Id == update.Id)
                    .Include(rc => rc.RecordingCreditRoles).ThenInclude(rcr => rcr.RecordingRole).SingleOrDefault();

                if (recordingCredit == null)
                    return new TaskResult<Nothing>(null as Nothing);

                foreach (var recordingCreditRole in recordingCredit.RecordingCreditRoles)
                {
                    var role = update.Roles.SingleOrDefault(r => r?.Id == recordingCreditRole.RecordingRoleId);
                    if (role == null)
                        _dbContext.RecordingCreditRoles.Remove(recordingCreditRole);
                }
                _dbContext.SaveChanges();

                foreach (var recordingRole in update.Roles.Where(role => role != null))
                {
                    var recordingCreditRole = recordingCredit.RecordingCreditRoles.SingleOrDefault(rcr => rcr.RecordingRoleId == recordingRole.Id);
                    if (recordingCreditRole == null)
                        recordingCredit.RecordingCreditRoles.Add(new RecordingCreditRole{ RecordingCreditId = recordingCredit.Id, RecordingRoleId = recordingRole.Id });
                }
                _dbContext.SaveChanges();

                return new TaskResult<Nothing>(true);
            }
            catch (Exception e)
            {
                return new TaskResult<Nothing>(new TaskException(e));
            }
        }
    }
}
