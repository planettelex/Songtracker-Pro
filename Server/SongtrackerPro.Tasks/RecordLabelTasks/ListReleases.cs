using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.RecordLabelTasks
{
    public interface IListReleasesTask : ITask<RecordLabel, List<Release>> { }

    public class ListReleases : TaskBase, IListReleasesTask
    {
        public ListReleases(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;


        public TaskResult<List<Release>> DoTask(RecordLabel recordLabel)
        {
            try
            {
                var releases = _dbContext.Releases.Where(r => r.RecordLabelId == recordLabel.Id)
                    .Include(r => r.Artist)
                    .Include(r => r.Genre)
                    .ToList();

                return new TaskResult<List<Release>>(releases);
            }
            catch (Exception e)
            {
                return new TaskResult<List<Release>>(new TaskException(e));
            }
        }
    }
}
