using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.MerchandiseTasks
{
    public interface IUpdateMerchandiseItemTask : ITask<MerchandiseItem, Nothing> { }

    public class UpdateMerchandiseItem : TaskBase, IUpdateMerchandiseItemTask
    {
        public UpdateMerchandiseItem(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<Nothing> DoTask(MerchandiseItem update)
        {
            try
            {
                var artistId = update.Artist?.Id ?? update.ArtistId;
                var artist = _dbContext.Artists.SingleOrDefault(a => a.Id == artistId);
                var categoryId = update.Category?.Id ?? update.CategoryId;
                var category = _dbContext.MerchandiseCategories.SingleOrDefault(mc => mc.Id == categoryId);
                var merchandiseItem = _dbContext.Merchandise.Single(mi => mi.Id == update.Id);
                merchandiseItem.Artist = artist;
                merchandiseItem.ArtistId = artistId;
                merchandiseItem.Category = category;
                merchandiseItem.CategoryId = categoryId;
                merchandiseItem.Name = update.Name;
                merchandiseItem.Description = update.Description;
                merchandiseItem.IsPromotional = update.IsPromotional;
                _dbContext.SaveChanges();
                
                return new TaskResult<Nothing>(true);
            }
            catch (Exception e)
            {
                return new TaskResult<Nothing>(new TaskException(e));
            }
        }
    }
}
