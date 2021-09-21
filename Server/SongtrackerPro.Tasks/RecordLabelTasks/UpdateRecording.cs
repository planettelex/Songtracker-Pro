using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.RecordLabelTasks
{
    public interface IUpdateRecordingTask : ITask<Recording, Nothing> { }

    public class UpdateRecording : TaskBase, IUpdateRecordingTask
    {
        public UpdateRecording(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<Nothing> DoTask(Recording update)
        {
            try
            {
                var recording = _dbContext.Recordings
                    .SingleOrDefault(r => r.Id == update.Id);

                if (recording == null)
                    throw new TaskException(SystemMessage("RECORDING_NOT_FOUND"));

                recording.IsCover = update.IsCover;
                recording.IsLive = update.IsLive;
                recording.IsRemix = update.IsRemix;
                recording.Isrc = update.Isrc;
                recording.SecondsLong = update.SecondsLong;
                recording.Title = update.Title;

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
