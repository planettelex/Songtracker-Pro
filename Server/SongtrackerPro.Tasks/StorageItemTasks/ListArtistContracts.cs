using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.StorageItemTasks
{
    public interface IListArtistContractsTask : ITask<Artist, List<Contract>> { }

    public class ListArtistContracts : TaskBase, IListArtistContractsTask
    {
        public ListArtistContracts(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<List<Contract>> DoTask(Artist artist)
        {
            try
            {
                var contracts = _dbContext.Contracts.Where(c => c.ArtistId == artist.Id)
                    .Include(c => c.Artist)
                    .Include(c => c.Publisher).ThenInclude(p => p.PerformingRightsOrganization)
                    .Include(c => c.RecordLabel)
                    .Include(c => c.Template)
                    .ToList();

                return new TaskResult<List<Contract>>(contracts);
            }
            catch (Exception e)
            {
                return new TaskResult<List<Contract>>(new TaskException(e));
            }
        }
    }
}
