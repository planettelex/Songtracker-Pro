using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.ArtistTasks
{
    public interface IListArtistAccountsTask : ITask<Artist, List<ArtistAccount>> { }

    public class ListArtistAccounts : IListArtistAccountsTask
    {
        public ListArtistAccounts(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<List<ArtistAccount>> DoTask(Artist artist)
        {
            try
            {
                var artistAccounts = _dbContext.ArtistAccounts.Where(aa => aa.ArtistId == artist.Id)
                    .Include(aa => aa.Platform)
                    .ToList();

                foreach (var _ in artistAccounts.Select(aa => _dbContext.PlatformServices
                    .Where(ps => ps.PlatformId == aa.PlatformId)
                    .Include(ps => ps.Service)
                    .ToList())) { }

                return new TaskResult<List<ArtistAccount>>(artistAccounts);
            }
            catch (Exception e)
            {
                return new TaskResult<List<ArtistAccount>>(new TaskException(e));
            }
        }
    }
}