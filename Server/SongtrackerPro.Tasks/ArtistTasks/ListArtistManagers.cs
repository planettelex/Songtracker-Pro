using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.ArtistTasks
{
    public interface IListArtistManagersTask : ITask<Artist, List<ArtistManager>> { }

    public class ListArtistManagers : TaskBase, IListArtistManagersTask
    {
        public ListArtistManagers(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<List<ArtistManager>> DoTask(Artist artist)
        {
            try
            {
                var artistManagers = _dbContext.ArtistManagers.Where(am => am.ArtistId == artist.Id)
                    .Include(am => am.Manager).ThenInclude(m => m.Address).ThenInclude(a => a.Country)
                    .ToList();

                return new TaskResult<List<ArtistManager>>(artistManagers);
            }
            catch (Exception e)
            {
                return new TaskResult<List<ArtistManager>>(new TaskException(e));
            }
        }
    }
}