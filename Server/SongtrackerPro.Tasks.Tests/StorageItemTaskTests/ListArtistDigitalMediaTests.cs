using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.ArtistTasks;
using SongtrackerPro.Tasks.StorageItemTasks;

namespace SongtrackerPro.Tasks.Tests.StorageItemTaskTests
{
    [TestClass]
    public class ListArtistDigitalMediaTests : TestsBase
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

            var addStorageTask = new AddStorageItem(DbContext);
            var testItem1 = TestsModel.DigitalMedia(testArtist, null);
            var addStorageResult = addStorageTask.DoTask(testItem1);

            Assert.IsTrue(addStorageResult.Success);
            Assert.IsNull(addStorageResult.Exception);
            Assert.IsNotNull(addStorageResult.Data);

            var storageItem1Id = addStorageResult.Data;
            Assert.IsNotNull(storageItem1Id);
            Assert.IsTrue(storageItem1Id.Value != Guid.Empty);

            addStorageTask = new AddStorageItem(DbContext);
            var testItem2 = TestsModel.DigitalMedia(testArtist, null);
            addStorageResult = addStorageTask.DoTask(testItem2);

            Assert.IsTrue(addStorageResult.Success);
            Assert.IsNull(addStorageResult.Exception);
            Assert.IsNotNull(addStorageResult.Data);

            var storageItem2Id = addStorageResult.Data;
            Assert.IsNotNull(storageItem2Id);
            Assert.IsTrue(storageItem2Id.Value != Guid.Empty);

            var task = new ListArtistDigitalMedia(DbContext);
            var result = task.DoTask(testArtist);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);

            var artistDigitalMedia = result.Data;
            Assert.IsNotNull(artistDigitalMedia);
            Assert.IsTrue(artistDigitalMedia.Count >= 2);

            var storageItem1 = artistDigitalMedia.SingleOrDefault(dm => dm.Uuid == storageItem1Id);
            Assert.IsNotNull(storageItem1);
            Assert.AreEqual(testItem1.Name, storageItem1.Name);
            Assert.AreEqual(testItem1.Container, storageItem1.Container);
            Assert.AreEqual(testItem1.FileName, storageItem1.FileName);
            Assert.AreEqual(testItem1.FolderPath, storageItem1.FolderPath);
            Assert.AreEqual(testItem1.IsCompressed, storageItem1.IsCompressed);
            Assert.AreEqual(testItem1.MediaCategory, storageItem1.MediaCategory);
            Assert.IsNotNull(storageItem1.Artist);
            Assert.AreEqual(testArtist.Name, storageItem1.Artist.Name);
            Assert.AreEqual(testArtist.Email, storageItem1.Artist.Email);

            var storageItem2 = artistDigitalMedia.SingleOrDefault(dm => dm.Uuid == storageItem2Id);
            Assert.IsNotNull(storageItem2);
            Assert.AreEqual(testItem2.Name, storageItem2.Name);
            Assert.AreEqual(testItem2.Container, storageItem2.Container);
            Assert.AreEqual(testItem2.FileName, storageItem2.FileName);
            Assert.AreEqual(testItem2.FolderPath, storageItem2.FolderPath);
            Assert.AreEqual(testItem2.IsCompressed, storageItem2.IsCompressed);
            Assert.AreEqual(testItem2.MediaCategory, storageItem2.MediaCategory);
            Assert.IsNotNull(storageItem2.Artist);
            Assert.AreEqual(testArtist.Name, storageItem2.Artist.Name);
            Assert.AreEqual(testArtist.Email, storageItem2.Artist.Email);

            var removeStorageItemTask = new RemoveStorageItem(DbContext);
            var removeStorageItemResult = removeStorageItemTask.DoTask(storageItem1);

            Assert.IsTrue(removeStorageItemResult.Success);
            Assert.IsNull(removeStorageItemResult.Exception);

            removeStorageItemTask = new RemoveStorageItem(DbContext);
            removeStorageItemResult = removeStorageItemTask.DoTask(storageItem2);

            Assert.IsTrue(removeStorageItemResult.Success);
            Assert.IsNull(removeStorageItemResult.Exception);

            var removeArtistTask = new RemoveArtist(DbContext);
            var removeArtistResult = removeArtistTask.DoTask(testArtist);

            Assert.IsTrue(removeArtistResult.Success);
            Assert.IsNull(removeArtistResult.Exception);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new ListArtistDigitalMedia(EmptyDbContext);
            var result = task.DoTask(null);
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
