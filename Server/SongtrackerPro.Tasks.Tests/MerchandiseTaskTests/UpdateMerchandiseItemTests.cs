using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.ArtistTasks;
using SongtrackerPro.Tasks.MerchandiseTasks;

namespace SongtrackerPro.Tasks.Tests.MerchandiseTaskTests
{
    [TestClass]
    public class UpdateMerchandiseItemTests : TestsBase
    {
        private MerchandiseCategory UpdateMerchandiseItemModel(MerchandiseItem merchandiseItem, Artist artist)
        {
            var stamp = DateTime.Now.Ticks;
            var merchandiseCategory = TestsModel.MerchandiseCategory;
            merchandiseItem.Name = "Update " + stamp;
            merchandiseItem.Description = "Description " + stamp;
            merchandiseItem.Artist = artist;
            merchandiseItem.Category = merchandiseCategory;
            merchandiseItem.IsPromotional = TestsModel.FiftyFifty;
            return merchandiseCategory;
        }

        [TestMethod]
        public void TaskSuccessTest()
        {
            var testMerchandiseItem = TestsModel.MerchandiseItem(null);
            var addMerchandiseItemTask = new AddMerchandiseItem(DbContext);
            var addMerchandiseItemResult = addMerchandiseItemTask.DoTask(testMerchandiseItem);

            Assert.IsTrue(addMerchandiseItemResult.Success);
            Assert.IsNull(addMerchandiseItemResult.Exception);

            var merchandiseItemId = addMerchandiseItemResult.Data;
            Assert.IsNotNull(merchandiseItemId);
            Assert.IsTrue(merchandiseItemId > 0);

            var testArtist = TestsModel.Artist;
            var addArtistTask = new AddArtist(DbContext, new FormattingService());
            var addArtistResult = addArtistTask.DoTask(testArtist);

            Assert.IsTrue(addArtistResult.Success);
            Assert.IsNull(addArtistResult.Exception);
            Assert.IsNotNull(addArtistResult.Data);

            var artistId = addArtistResult.Data;
            Assert.IsNotNull(artistId);
            Assert.IsTrue(artistId > 0);

            var task = new UpdateMerchandiseItem(DbContext);
            var testCategory = UpdateMerchandiseItemModel(testMerchandiseItem, testArtist);
            var result = task.DoTask(testMerchandiseItem);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNull(result.Data);

            var getMerchandiseItemTask = new GetMerchandiseItem(DbContext);
            var merchandiseItem = getMerchandiseItemTask.DoTask(merchandiseItemId.Value)?.Data;

            Assert.IsNotNull(merchandiseItem);
            Assert.AreEqual(testMerchandiseItem.Name, merchandiseItem.Name);
            Assert.AreEqual(testMerchandiseItem.Description, merchandiseItem.Description);
            Assert.AreEqual(testMerchandiseItem.IsPromotional, merchandiseItem.IsPromotional);
            Assert.IsNotNull(merchandiseItem.Artist);
            Assert.AreEqual(testArtist.Name, merchandiseItem.Artist.Name);
            Assert.AreEqual(testArtist.Email, merchandiseItem.Artist.Email);
            Assert.IsNotNull(merchandiseItem.Category);
            Assert.AreEqual(testCategory.Name, merchandiseItem.Category.Name);
            Assert.AreEqual(testCategory.Description, merchandiseItem.Category.Description);

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
        public void TaskFailTest()
        {
            var task = new UpdateMerchandiseItem(EmptyDbContext);
            var result = task.DoTask(null);
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
