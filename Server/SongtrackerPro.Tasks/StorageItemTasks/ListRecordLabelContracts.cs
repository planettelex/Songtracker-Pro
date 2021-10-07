using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.StorageItemTasks
{
    public interface IListRecordLabelContractsTask : ITask<RecordLabel, List<RecordLabelContract>> { }

    public class ListRecordLabelContracts : TaskBase, IListRecordLabelContractsTask
    {
        public ListRecordLabelContracts(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<List<RecordLabelContract>> DoTask(RecordLabel recordLabel)
        {
            try
            {
                var contracts = _dbContext.RecordLabelContracts.Where(rlc => rlc.RecordLabelId == recordLabel.Id)
                    .Include(rlc => rlc.Release).ThenInclude(r => r.Artist)
                    .Include(rlc => rlc.Release).ThenInclude(r => r.Genre)
                    .Include(rlc => rlc.Release).ThenInclude(r => r.RecordLabel)
                    .Include(rlc => rlc.Recording).ThenInclude(r => r.Composition).ThenInclude(c => c.Publisher)
                    .Include(rlc => rlc.Recording).ThenInclude(r => r.Composition).ThenInclude(c => c.ExternalPublisher)
                    .Include(rlc => rlc.Recording).ThenInclude(r => r.Artist)
                    .Include(rlc => rlc.Recording).ThenInclude(r => r.Genre)
                    .Include(rlc => rlc.Recording).ThenInclude(r => r.RecordLabel)
                    .Include(rlc => rlc.Recording).ThenInclude(r => r.OriginalRecording)
                    .Include(rlc => rlc.Artist)
                    .Include(rlc => rlc.RecordLabel)
                    .Include(rlc => rlc.Template)
                    .ToList();

                return new TaskResult<List<RecordLabelContract>>(contracts);
            }
            catch (Exception e)
            {
                return new TaskResult<List<RecordLabelContract>>(new TaskException(e));
            }
        }
    }
}
