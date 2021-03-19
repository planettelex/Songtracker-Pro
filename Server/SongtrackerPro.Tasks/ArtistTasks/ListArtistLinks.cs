using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.ArtistTasks
{
    public interface IListArtistLinksTask : ITask<Artist, List<ArtistLink>> { }

    public class ListArtistLinks : TaskBase, IListArtistLinksTask
    {
        public ListArtistLinks(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<List<ArtistLink>> DoTask(Artist artist)
        {
            try
            {
                var artistLinks = _dbContext.ArtistLinks.Where(al => al.ArtistId == artist.Id)
                    .Include(al => al.Platform)
                    .ToList();

                foreach (var _ in artistLinks.Select(al => _dbContext.PlatformServices
                    .Where(ps => ps.PlatformId == al.PlatformId)
                    .Include(ps => ps.Service)
                    .ToList())) { }

                return new TaskResult<List<ArtistLink>>(artistLinks);
            }
            catch (Exception e)
            {
                return new TaskResult<List<ArtistLink>>(new TaskException(e));
            }
        }
    }
}