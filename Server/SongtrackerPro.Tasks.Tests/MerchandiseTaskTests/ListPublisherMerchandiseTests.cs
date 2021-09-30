using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.MerchandiseTasks;
using SongtrackerPro.Tasks.PublishingTasks;

namespace SongtrackerPro.Tasks.Tests.MerchandiseTaskTests
{
    [TestClass]
    public class ListPublisherMerchandiseTests : TestsBase
    {
        [TestMethod]
        public void TaskSuccessTest()
        {
            var testPublisher = TestsModel.Publisher;
            var addPublisherTask = new AddPublisher(DbContext, new FormattingService());
            var addPublisherResult = addPublisherTask.DoTask(testPublisher);

            Assert.IsTrue(addPublisherResult.Success);
            Assert.IsNull(addPublisherResult.Exception);

            var testMerchandiseItem1 = TestsModel.PublisherMerchandiseItem(testPublisher);
            var addMerchandiseItemTask = new AddMerchandiseItem(DbContext);
            var addMerchandiseItemResult = addMerchandiseItemTask.DoTask(testMerchandiseItem1);

            Assert.IsTrue(addMerchandiseItemResult.Success);
            Assert.IsNull(addMerchandiseItemResult.Exception);
            Assert.IsNotNull(addMerchandiseItemResult.Data);

            var merchandiseItem1Id = addMerchandiseItemResult.Data;
            Assert.IsNotNull(merchandiseItem1Id);
            Assert.IsTrue(merchandiseItem1Id > 0);

            var testMerchandiseItem2 = TestsModel.PublisherMerchandiseItem(testPublisher);
            addMerchandiseItemTask = new AddMerchandiseItem(DbContext);
            addMerchandiseItemResult = addMerchandiseItemTask.DoTask(testMerchandiseItem2);

            Assert.IsTrue(addMerchandiseItemResult.Success);
            Assert.IsNull(addMerchandiseItemResult.Exception);
            Assert.IsNotNull(addMerchandiseItemResult.Data);

            var merchandiseItem2Id = addMerchandiseItemResult.Data;
            Assert.IsNotNull(merchandiseItem2Id);
            Assert.IsTrue(merchandiseItem2Id > 0);

            var task = new ListPublisherMerchandise(DbContext);
            var result = task.DoTask(testPublisher);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);

            var merchandiseItems = result.Data;
            Assert.IsNotNull(merchandiseItems);
            Assert.IsTrue(merchandiseItems.Count >= 2);

            var merchandiseItem1 = merchandiseItems.SingleOrDefault(mi => mi.Id == merchandiseItem1Id);
            Assert.IsNotNull(merchandiseItem1);
            Assert.AreEqual(testMerchandiseItem1.Name, merchandiseItem1.Name);
            Assert.AreEqual(testMerchandiseItem1.Description, merchandiseItem1.Description);
            Assert.AreEqual(testMerchandiseItem1.IsPromotional, merchandiseItem1.IsPromotional);
            Assert.IsNotNull(merchandiseItem1.Publisher);
            Assert.AreEqual(testPublisher.Name, merchandiseItem1.Publisher.Name);
            Assert.IsNotNull(merchandiseItem1.Category);
            Assert.IsNotNull(merchandiseItem1.Category.Name);

            var merchandiseItem2 = merchandiseItems.SingleOrDefault(mi => mi.Id == merchandiseItem2Id);
            Assert.IsNotNull(merchandiseItem2);
            Assert.AreEqual(testMerchandiseItem2.Name, merchandiseItem2.Name);
            Assert.AreEqual(testMerchandiseItem2.Description, merchandiseItem2.Description);
            Assert.AreEqual(testMerchandiseItem2.IsPromotional, merchandiseItem2.IsPromotional);
            Assert.IsNotNull(merchandiseItem2.Publisher);
            Assert.AreEqual(testPublisher.Name, merchandiseItem2.Publisher.Name);
            Assert.IsNotNull(merchandiseItem2.Category);
            Assert.IsNotNull(merchandiseItem2.Category.Name);

            var removeMerchandiseItemTask = new RemoveMerchandiseItem(DbContext);
            var removeResult = removeMerchandiseItemTask.DoTask(merchandiseItem1);

            Assert.IsTrue(removeResult.Success);
            Assert.IsNull(removeResult.Exception);

            removeMerchandiseItemTask = new RemoveMerchandiseItem(DbContext);
            removeResult = removeMerchandiseItemTask.DoTask(merchandiseItem2);

            Assert.IsTrue(removeResult.Success);
            Assert.IsNull(removeResult.Exception);

            var removePublisherTask = new RemovePublisher(DbContext);
            var removeArtistResult = removePublisherTask.DoTask(testPublisher);

            Assert.IsTrue(removeArtistResult.Success);
            Assert.IsNull(removeArtistResult.Exception);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new ListPublisherMerchandise(EmptyDbContext);
            var result = task.DoTask(new Publisher());
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
