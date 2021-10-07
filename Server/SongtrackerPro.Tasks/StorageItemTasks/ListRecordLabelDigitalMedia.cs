using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.StorageItemTasks
{
    public interface IListRecordLabelDigitalMediaTask : ITask<RecordLabel, List<DigitalMedia>> { }

    public class ListRecordLabelDigitalMedia : TaskBase, IListRecordLabelDigitalMediaTask
    {
        public ListRecordLabelDigitalMedia(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<List<DigitalMedia>> DoTask(RecordLabel recordLabel)
        {
            try
            {
                var digitalMedia = _dbContext.DigitalMedia.Where(dm => dm.RecordLabelId == recordLabel.Id)
                    .Include(dm => dm.Artist)
                    .Include(dm => dm.Publisher).ThenInclude(p => p.PerformingRightsOrganization)
                    .Include(dm => dm.RecordLabel)
                    .ToList();

                return new TaskResult<List<DigitalMedia>>(digitalMedia);
            }
            catch (Exception e)
            {
                return new TaskResult<List<DigitalMedia>>(new TaskException(e));
            }
        }
    }
}
