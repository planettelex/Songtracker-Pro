using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.RecordLabelTasks
{
    public interface IGetRecordingTask : ITask<int, Recording> { }

    public class GetRecording : TaskBase, IGetRecordingTask
    {
        public GetRecording(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<Recording> DoTask(int recordingId)
        {
            try
            {
                var recording = _dbContext.Recordings.Where(r => r.Id == recordingId)
                    .Include(r => r.Artist).ThenInclude(a => a.Address).ThenInclude(a => a.Country)
                    .Include(r => r.Artist).ThenInclude(a => a.RecordLabel).ThenInclude(rl => rl.Address).ThenInclude(a => a.Country)
                    .Include(r => r.RecordLabel).ThenInclude(rl => rl.Address).ThenInclude(a => a.Country)
                    .Include(r => r.Composition).ThenInclude(c => c.Publisher).ThenInclude(p => p.Address).ThenInclude(a => a.Country)
                    .Include(r => r.Composition).ThenInclude(c => c.Publisher).ThenInclude(p => p.PerformingRightsOrganization).ThenInclude(o => o.Country)
                    .Include(r => r.Composition).ThenInclude(c => c.ExternalPublisher).ThenInclude(ep => ep.Address).ThenInclude(a => a.Country)
                    .Include(r => r.OriginalRecording).ThenInclude(or => or.Artist).ThenInclude(a => a.Address).ThenInclude(a => a.Country)
                    .Include(r => r.OriginalRecording).ThenInclude(or => or.Composition).ThenInclude(c => c.Publisher).ThenInclude(p => p.Address).ThenInclude(a => a.Country)
                    .Include(r => r.OriginalRecording).ThenInclude(or => or.RecordLabel).ThenInclude(rl => rl.Address).ThenInclude(a => a.Country)
                    .SingleOrDefault();

                return new TaskResult<Recording>(recording);
            }
            catch (Exception e)
            {
                return new TaskResult<Recording>(new TaskException(e));
            }
        }
    }
}
