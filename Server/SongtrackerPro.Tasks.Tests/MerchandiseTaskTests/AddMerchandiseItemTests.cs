using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.ArtistTasks;
using SongtrackerPro.Tasks.MerchandiseTasks;
using SongtrackerPro.Tasks.PublishingTasks;
using SongtrackerPro.Tasks.RecordLabelTasks;

namespace SongtrackerPro.Tasks.Tests.MerchandiseTaskTests
{
    [TestClass]
    public class AddMerchandiseItemTests : TestsBase
    {
        [TestMethod]
        public void TaskBaseSuccessTest()
        {
            var testArtist = TestsModel.Artist;
            var addArtistTask = new AddArtist(DbContext, new FormattingService());
            var addArtistResult = addArtistTask.DoTask(testArtist);

            Assert.IsTrue(addArtistResult.Success);
            Assert.IsNull(addArtistResult.Exception);
            Assert.IsNotNull(addArtistResult.Data);

            var artistId = addArtistResult.Data;
            Assert.IsNotNull(artistId);
            Assert.IsTrue(artistId > 0);

            var task = new AddMerchandiseItem(DbContext);
            var testItem = TestsModel.MerchandiseItem(testArtist);
            var result = task.DoTask(testItem);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Data);

            var merchandiseItemId = result.Data;
            Assert.IsNotNull(merchandiseItemId);
            Assert.IsTrue(merchandiseItemId > 0);

            var getMerchandiseItemTask = new GetMerchandiseItem(DbContext);
            var merchandiseItem = getMerchandiseItemTask.DoTask(merchandiseItemId.Value)?.Data;

            Assert.IsNotNull(merchandiseItem);
            Assert.AreEqual(testItem.Name, merchandiseItem.Name);
            Assert.AreEqual(testItem.Description, merchandiseItem.Description);
            Assert.AreEqual(testItem.IsPromotional, merchandiseItem.IsPromotional);
            Assert.IsNotNull(merchandiseItem.Artist);
            Assert.AreEqual(testArtist.Name, merchandiseItem.Artist.Name);
            Assert.AreEqual(testArtist.Email, merchandiseItem.Artist.Email);
            Assert.IsNotNull(testItem.Category);
            Assert.IsNotNull(merchandiseItem.Category);
            Assert.AreEqual(testItem.Category.Name, merchandiseItem.Category.Name);
            Assert.AreEqual(testItem.Category.Description, merchandiseItem.Category.Description);

            var removeMerchandiseItemTask = new RemoveMerchandiseItem(DbContext);
            var removeResult = removeMerchandiseItemTask.DoTask(merchandiseItem);

            Assert.IsTrue(removeResult.Success);
            Assert.IsNull(removeResult.Exception);

            var removeArtistTask = new RemoveArtist(DbContext);
            var removeArtistResult = removeArtistTask.DoTask(testArtist);

            Assert.IsTrue(removeArtistResult.Success);
            Assert.IsNull(removeArtistResult.Exception);
        }

        [TestMethod]
        public void TaskPublisherSuccessTest()
        {
            var testPublisher = TestsModel.Publisher;
            var addPublisherTask = new AddPublisher(DbContext, new FormattingService());
            var addPublisherResult = addPublisherTask.DoTask(testPublisher);

            Assert.IsTrue(addPublisherResult.Success);
            Assert.IsNull(addPublisherResult.Exception);
            Assert.IsNotNull(addPublisherResult.Data);

            var publisherId = addPublisherResult.Data;
            Assert.IsNotNull(publisherId);
            Assert.IsTrue(publisherId > 0);

            var task = new AddMerchandiseItem(DbContext);
            var testItem = TestsModel.PublisherMerchandiseItem(testPublisher);
            var result = task.DoTask(testItem);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Data);

            var merchandiseItemId = result.Data;
            Assert.IsNotNull(merchandiseItemId);
            Assert.IsTrue(merchandiseItemId > 0);

            var getMerchandiseItemTask = new GetMerchandiseItem(DbContext);
            var merchandiseItem = getMerchandiseItemTask.DoTask(merchandiseItemId.Value)?.Data as PublisherMerchandiseItem;

            Assert.IsNotNull(merchandiseItem);
            Assert.AreEqual(testItem.Name, merchandiseItem.Name);
            Assert.AreEqual(testItem.Description, merchandiseItem.Description);
            Assert.AreEqual(testItem.IsPromotional, merchandiseItem.IsPromotional);
            Assert.IsNull(merchandiseItem.Artist);
            Assert.IsNotNull(merchandiseItem.Publisher);
            Assert.AreEqual(testPublisher.Name, merchandiseItem.Publisher.Name);
            Assert.AreEqual(testPublisher.Email, merchandiseItem.Publisher.Email);
            Assert.IsNotNull(testItem.Category);
            Assert.IsNotNull(merchandiseItem.Category);
            Assert.AreEqual(testItem.Category.Name, merchandiseItem.Category.Name);
            Assert.AreEqual(testItem.Category.Description, merchandiseItem.Category.Description);

            var removeMerchandiseItemTask = new RemoveMerchandiseItem(DbContext);
            var removeResult = removeMerchandiseItemTask.DoTask(merchandiseItem);

            Assert.IsTrue(removeResult.Success);
            Assert.IsNull(removeResult.Exception);

            var removePublisherTask = new RemovePublisher(DbContext);
            var removePublisherResult = removePublisherTask.DoTask(testPublisher);

            Assert.IsTrue(removePublisherResult.Success);
            Assert.IsNull(removePublisherResult.Exception);
        }

        [TestMethod]
        public void TaskRecordLabelSuccessTest()
        {
            var testArtist = TestsModel.Artist;
            var addArtistTask = new AddArtist(DbContext, new FormattingService());
            var addArtistResult = addArtistTask.DoTask(testArtist);

            Assert.IsTrue(addArtistResult.Success);
            Assert.IsNull(addArtistResult.Exception);
            Assert.IsNotNull(addArtistResult.Data);

            var artistId = addArtistResult.Data;
            Assert.IsNotNull(artistId);
            Assert.IsTrue(artistId > 0);

            var testRecordLabel = TestsModel.RecordLabel;
            var addRecordLabelTask = new AddRecordLabel(DbContext, new FormattingService());
            var addRecordLabelResult = addRecordLabelTask.DoTask(testRecordLabel);

            Assert.IsTrue(addRecordLabelResult.Success);
            Assert.IsNull(addRecordLabelResult.Exception);
            Assert.IsNotNull(addRecordLabelResult.Data);

            var recordLabelId = addRecordLabelResult.Data;
            Assert.IsNotNull(recordLabelId);
            Assert.IsTrue(recordLabelId > 0);

            var task = new AddMerchandiseItem(DbContext);
            var testItem = TestsModel.RecordLabelMerchandiseItem(testRecordLabel, testArtist);
            var result = task.DoTask(testItem);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Data);

            var merchandiseItemId = result.Data;
            Assert.IsNotNull(merchandiseItemId);
            Assert.IsTrue(merchandiseItemId > 0);

            var getMerchandiseItemTask = new GetMerchandiseItem(DbContext);
            var merchandiseItem = getMerchandiseItemTask.DoTask(merchandiseItemId.Value)?.Data as RecordLabelMerchandiseItem;

            Assert.IsNotNull(merchandiseItem);
            Assert.AreEqual(testItem.Name, merchandiseItem.Name);
            Assert.AreEqual(testItem.Description, merchandiseItem.Description);
            Assert.AreEqual(testItem.IsPromotional, merchandiseItem.IsPromotional);
            Assert.IsNotNull(merchandiseItem.Artist);
            Assert.AreEqual(testArtist.Name, merchandiseItem.Artist.Name);
            Assert.AreEqual(testArtist.Email, merchandiseItem.Artist.Email);
            Assert.IsNotNull(merchandiseItem.RecordLabel);
            Assert.AreEqual(testRecordLabel.Name, merchandiseItem.RecordLabel.Name);
            Assert.AreEqual(testRecordLabel.Email, merchandiseItem.RecordLabel.Email);
            Assert.IsNotNull(testItem.Category);
            Assert.IsNotNull(merchandiseItem.Category);
            Assert.AreEqual(testItem.Category.Name, merchandiseItem.Category.Name);
            Assert.AreEqual(testItem.Category.Description, merchandiseItem.Category.Description);

            var removeMerchandiseItemTask = new RemoveMerchandiseItem(DbContext);
            var removeResult = removeMerchandiseItemTask.DoTask(merchandiseItem);

            Assert.IsTrue(removeResult.Success);
            Assert.IsNull(removeResult.Exception);

            var removeRecordLabelTask = new RemoveRecordLabel(DbContext);
            var removeRecordLabelResult = removeRecordLabelTask.DoTask(testRecordLabel);

            Assert.IsTrue(removeRecordLabelResult.Success);
            Assert.IsNull(removeRecordLabelResult.Exception);

            var removeArtistTask = new RemoveArtist(DbContext);
            var removeArtistResult = removeArtistTask.DoTask(testArtist);

            Assert.IsTrue(removeArtistResult.Success);
            Assert.IsNull(removeArtistResult.Exception);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new AddMerchandiseItem(EmptyDbContext);
            var result = task.DoTask(new MerchandiseItem());
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
