using System;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Tasks.MerchandiseTasks
{
    public interface IAddMerchandiseItemTask : ITask<MerchandiseItem, int?> { }

    public class AddMerchandiseItem : TaskBase, IAddMerchandiseItemTask
    {
        public AddMerchandiseItem(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        private readonly ApplicationDbContext _dbContext;

        public TaskResult<int?> DoTask(MerchandiseItem merchandiseItem)
        {
            try
            {
                var artistId = merchandiseItem.Artist?.Id ?? merchandiseItem.ArtistId;
                var artist = _dbContext.Artists.SingleOrDefault(a => a.Id == artistId);
                var categoryId = merchandiseItem.Category?.Id ?? merchandiseItem.CategoryId;
                var category = _dbContext.MerchandiseCategories.SingleOrDefault(mc => mc.Id == categoryId);

                merchandiseItem.Artist = null;
                merchandiseItem.ArtistId = artistId;
                merchandiseItem.Category = null;
                merchandiseItem.CategoryId = categoryId;

                if (merchandiseItem is PublisherMerchandiseItem publisherMerchandiseItem)
                {
                    var publisherId = publisherMerchandiseItem.Publisher?.Id ?? publisherMerchandiseItem.PublisherId;
                    var publisher = _dbContext.Publishers.Single(p => p.Id == publisherId);

                    publisherMerchandiseItem.Publisher = null;
                    publisherMerchandiseItem.PublisherId = publisherId;

                    _dbContext.PublisherMerchandise.Add(publisherMerchandiseItem);
                    _dbContext.SaveChanges();

                    publisherMerchandiseItem.Publisher = publisher;
                }
                else if (merchandiseItem is RecordLabelMerchandiseItem recordLabelMerchandiseItem)
                {
                    var recordLabelId = recordLabelMerchandiseItem.RecordLabel?.Id ?? recordLabelMerchandiseItem.RecordLabelId;
                    var recordLabel = _dbContext.RecordLabels.Single(rl => rl.Id == recordLabelId);

                    recordLabelMerchandiseItem.RecordLabel = null;
                    recordLabelMerchandiseItem.RecordLabelId = recordLabelId;

                    _dbContext.RecordLabelMerchandise.Add(recordLabelMerchandiseItem);
                    _dbContext.SaveChanges();

                    recordLabelMerchandiseItem.RecordLabel = recordLabel;
                }
                else
                {
                    _dbContext.MerchandiseItems.Add(merchandiseItem);
                    _dbContext.SaveChanges();
                }

                merchandiseItem.Artist = artist;
                merchandiseItem.Category = category;

                return new TaskResult<int?>(merchandiseItem.Id);
            }
            catch (Exception e)
            {
                return new TaskResult<int?>(new TaskException(e));
            }
        }
    }
}
