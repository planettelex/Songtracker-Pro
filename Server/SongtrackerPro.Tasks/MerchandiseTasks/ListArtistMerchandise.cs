using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.MerchandiseTasks
{
    public interface IListArtistMerchandiseTask : ITask<Artist, List<MerchandiseItem>> { }

    public class ListArtistMerchandise : TaskBase, IListArtistMerchandiseTask
    {
        public ListArtistMerchandise(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<List<MerchandiseItem>> DoTask(Artist artist)
        {
            try
            {
                var merchandiseItems = _dbContext.Merchandise.Where(mi => mi.ArtistId == artist.Id)
                    .Include(mi => mi.Artist)
                    .Include(mi => mi.Category)
                    .ToList();

                return new TaskResult<List<MerchandiseItem>>(merchandiseItems);
            }
            catch (Exception e)
            {
                return new TaskResult<List<MerchandiseItem>>(new TaskException(e));
            }
        }
    }
}
