using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.ArtistTasks
{
    public interface IListArtistsTask : ITask<Nothing, List<Artist>> { }

    public class ListArtists : TaskBase, IListArtistsTask
    {
        public ListArtists(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<List<Artist>> DoTask(Nothing nothing)
        {
            try
            {
                var artists = _dbContext.Artists
                    .Include(a => a.RecordLabel).ThenInclude(l => l.Address).ThenInclude(a => a.Country)
                    .ToList();

                return new TaskResult<List<Artist>>(artists);
            }
            catch (Exception e)
            {
                return new TaskResult<List<Artist>>(new TaskException(e));
            }
        }
    }
}