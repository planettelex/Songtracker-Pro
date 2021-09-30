using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.ArtistTasks;
using SongtrackerPro.Tasks.MerchandiseTasks;

namespace SongtrackerPro.Tasks.Tests.MerchandiseTaskTests
{
    [TestClass]
    public class ListArtistMerchandiseTests : TestsBase
    {
        [TestMethod]
        public void TaskSuccessTest()
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

            var testMerchandiseItem1 = TestsModel.MerchandiseItem(testArtist);
            var addMerchandiseItemTask = new AddMerchandiseItem(DbContext);
            var addMerchandiseItemResult = addMerchandiseItemTask.DoTask(testMerchandiseItem1);

            Assert.IsTrue(addMerchandiseItemResult.Success);
            Assert.IsNull(addMerchandiseItemResult.Exception);
            Assert.IsNotNull(addMerchandiseItemResult.Data);

            var merchandiseItem1Id = addMerchandiseItemResult.Data;
            Assert.IsNotNull(merchandiseItem1Id);
            Assert.IsTrue(merchandiseItem1Id > 0);

            var testMerchandiseItem2 = TestsModel.MerchandiseItem(testArtist);
            addMerchandiseItemTask = new AddMerchandiseItem(DbContext);
            addMerchandiseItemResult = addMerchandiseItemTask.DoTask(testMerchandiseItem2);

            Assert.IsTrue(addMerchandiseItemResult.Success);
            Assert.IsNull(addMerchandiseItemResult.Exception);
            Assert.IsNotNull(addMerchandiseItemResult.Data);

            var merchandiseItem2Id = addMerchandiseItemResult.Data;
            Assert.IsNotNull(merchandiseItem2Id);
            Assert.IsTrue(merchandiseItem2Id > 0);

            var task = new ListArtistMerchandise(DbContext);
            var result = task.DoTask(testArtist);

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
            Assert.IsNotNull(merchandiseItem1.Artist);
            Assert.AreEqual(testArtist.Name, merchandiseItem1.Artist.Name);
            Assert.IsNotNull(merchandiseItem1.Category);
            Assert.IsNotNull(merchandiseItem1.Category.Name);

            var merchandiseItem2 = merchandiseItems.SingleOrDefault(mi => mi.Id == merchandiseItem2Id);
            Assert.IsNotNull(merchandiseItem2);
            Assert.AreEqual(testMerchandiseItem2.Name, merchandiseItem2.Name);
            Assert.AreEqual(testMerchandiseItem2.Description, merchandiseItem2.Description);
            Assert.AreEqual(testMerchandiseItem2.IsPromotional, merchandiseItem2.IsPromotional);
            Assert.IsNotNull(merchandiseItem2.Artist);
            Assert.AreEqual(testArtist.Name, merchandiseItem2.Artist.Name);
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

            var removeArtistTask = new RemoveArtist(DbContext);
            var removeArtistResult = removeArtistTask.DoTask(testArtist);

            Assert.IsTrue(removeArtistResult.Success);
            Assert.IsNull(removeArtistResult.Exception);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new ListArtistMerchandise(EmptyDbContext);
            var result = task.DoTask(new Artist());
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
