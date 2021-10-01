using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.PublishingTasks;
using SongtrackerPro.Tasks.StorageItemTasks;

namespace SongtrackerPro.Tasks.Tests.StorageItemTaskTests
{
    [TestClass]
    public class ListPublisherContractsTests : TestsBase
    {
        [TestMethod]
        public void TaskSuccessTest()
        {
            var addPublisherTask = new AddPublisher(DbContext, new FormattingService());
            var testPublisher = TestsModel.Publisher;
            var addPublisherResult = addPublisherTask.DoTask(testPublisher);

            Assert.IsTrue(addPublisherResult.Success);
            Assert.IsNull(addPublisherResult.Exception);
            Assert.IsNotNull(addPublisherResult.Data);

            var publisherId = addPublisherResult.Data;
            Assert.IsNotNull(publisherId);
            Assert.IsTrue(publisherId > 0);

            var addStorageTask = new AddStorageItem(DbContext);
            var testItem1 = TestsModel.PublisherContract(testPublisher);
            var addStorageResult = addStorageTask.DoTask(testItem1);

            Assert.IsTrue(addStorageResult.Success);
            Assert.IsNull(addStorageResult.Exception);
            Assert.IsNotNull(addStorageResult.Data);

            var storageItem1Id = addStorageResult.Data;
            Assert.IsNotNull(storageItem1Id);
            Assert.IsTrue(storageItem1Id.Value != Guid.Empty);

            addStorageTask = new AddStorageItem(DbContext);
            var testItem2 = TestsModel.PublisherContract(testPublisher);
            addStorageResult = addStorageTask.DoTask(testItem2);

            Assert.IsTrue(addStorageResult.Success);
            Assert.IsNull(addStorageResult.Exception);
            Assert.IsNotNull(addStorageResult.Data);

            var storageItem2Id = addStorageResult.Data;
            Assert.IsNotNull(storageItem2Id);
            Assert.IsTrue(storageItem2Id.Value != Guid.Empty);

            var task = new ListPublisherContracts(DbContext);
            var result = task.DoTask(testPublisher);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);

            var publisherContracts = result.Data;
            Assert.IsNotNull(publisherContracts);
            Assert.IsTrue(publisherContracts.Count >= 2);

            var storageItem1 = publisherContracts.SingleOrDefault(dm => dm.Uuid == storageItem1Id);
            Assert.IsNotNull(storageItem1);
            Assert.AreEqual(testItem1.Name, storageItem1.Name);
            Assert.AreEqual(testItem1.Container, storageItem1.Container);
            Assert.AreEqual(testItem1.FileName, storageItem1.FileName);
            Assert.AreEqual(testItem1.FolderPath, storageItem1.FolderPath);
            Assert.AreEqual(testItem1.DocumentType, storageItem1.DocumentType);
            Assert.AreEqual(testItem1.Version, storageItem1.Version);
            Assert.IsNotNull(storageItem1.Publisher);
            Assert.AreEqual(testPublisher.Name, storageItem1.Publisher.Name);
            Assert.AreEqual(testPublisher.Email, storageItem1.Publisher.Email);

            var storageItem2 = publisherContracts.SingleOrDefault(dm => dm.Uuid == storageItem2Id);
            Assert.IsNotNull(storageItem2);
            Assert.AreEqual(testItem2.Name, storageItem2.Name);
            Assert.AreEqual(testItem2.Container, storageItem2.Container);
            Assert.AreEqual(testItem2.FileName, storageItem2.FileName);
            Assert.AreEqual(testItem2.FolderPath, storageItem2.FolderPath);
            Assert.AreEqual(testItem2.DocumentType, storageItem2.DocumentType);
            Assert.AreEqual(testItem2.Version, storageItem2.Version);
            Assert.IsNotNull(storageItem2.Publisher);
            Assert.AreEqual(testPublisher.Name, storageItem2.Publisher.Name);
            Assert.AreEqual(testPublisher.Email, storageItem2.Publisher.Email);

            var removeStorageItemTask = new RemoveStorageItem(DbContext);
            var removeStorageItemResult = removeStorageItemTask.DoTask(storageItem1);

            Assert.IsTrue(removeStorageItemResult.Success);
            Assert.IsNull(removeStorageItemResult.Exception);

            removeStorageItemTask = new RemoveStorageItem(DbContext);
            removeStorageItemResult = removeStorageItemTask.DoTask(storageItem2);

            Assert.IsTrue(removeStorageItemResult.Success);
            Assert.IsNull(removeStorageItemResult.Exception);

            var removePublisherTask = new RemovePublisher(DbContext);
            var removePublisherResult = removePublisherTask.DoTask(testPublisher);

            Assert.IsTrue(removePublisherResult.Success);
            Assert.IsNull(removePublisherResult.Exception);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new ListPublisherContracts(EmptyDbContext);
            var result = task.DoTask(null);
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
