using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.RecordLabelTasks
{
    public interface IListReleaseTracksTask : ITask<Release, List<ReleaseTrack>> { }

    public class ListReleaseTracks : TaskBase, IListReleaseTracksTask
    {
        public ListReleaseTracks(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<List<ReleaseTrack>> DoTask(Release release)
        {
            try
            {
                var tracks = _dbContext.ReleaseTracks.Where(rt => rt.ReleaseId == release.Id)
                    .Include(rt => rt.Recording)
                    .OrderBy(rt => rt.TrackNumber)
                    .ToList();

                return new TaskResult<List<ReleaseTrack>>(tracks);
            }
            catch (Exception e)
            {
                return new TaskResult<List<ReleaseTrack>>(new TaskException(e));
            }
        }
    }
}
