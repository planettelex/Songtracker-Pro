using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.InstallationTasks
{
    public interface IListGenresTask : ITask<Nothing, List<Genre>> { }

    public class ListGenres : TaskBase, IListGenresTask
    {
        public ListGenres(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<List<Genre>> DoTask(Nothing nothing)
        {
            try
            {
                var genres = _dbContext.Genres
                    .Include(g => g.ParentGenre).ThenInclude(pg => pg.ParentGenre)
                    .ToList();

                return new TaskResult<List<Genre>>(genres);
            }
            catch (Exception e)
            {
                return new TaskResult<List<Genre>>(new TaskException(e));
            }
        }
    }
}
