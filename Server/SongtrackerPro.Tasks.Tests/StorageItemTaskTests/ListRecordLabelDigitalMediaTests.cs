using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.RecordLabelTasks;
using SongtrackerPro.Tasks.StorageItemTasks;

namespace SongtrackerPro.Tasks.Tests.StorageItemTaskTests
{
    [TestClass]
    public class ListRecordLabelDigitalMediaTests : TestsBase
    {
        [TestMethod]
        public void TaskSuccessTest()
        {
            var testRecordLabel = TestsModel.RecordLabel;
            var addRecordLabelTask = new AddRecordLabel(DbContext, new FormattingService());
            var addRecordLabelResult = addRecordLabelTask.DoTask(testRecordLabel);

            Assert.IsTrue(addRecordLabelResult.Success);
            Assert.IsNull(addRecordLabelResult.Exception);
            Assert.IsNotNull(addRecordLabelResult.Data);

            var recordLabelId = addRecordLabelResult.Data;
            Assert.IsNotNull(recordLabelId);
            Assert.IsTrue(recordLabelId > 0);

            var addStorageTask = new AddStorageItem(DbContext);
            var testItem1 = TestsModel.DigitalMedia(null, testRecordLabel);
            var addStorageResult = addStorageTask.DoTask(testItem1);

            Assert.IsTrue(addStorageResult.Success);
            Assert.IsNull(addStorageResult.Exception);
            Assert.IsNotNull(addStorageResult.Data);

            var storageItem1Id = addStorageResult.Data;
            Assert.IsNotNull(storageItem1Id);
            Assert.IsTrue(storageItem1Id.Value != Guid.Empty);

            addStorageTask = new AddStorageItem(DbContext);
            var testItem2 = TestsModel.DigitalMedia(null, testRecordLabel);
            addStorageResult = addStorageTask.DoTask(testItem2);

            Assert.IsTrue(addStorageResult.Success);
            Assert.IsNull(addStorageResult.Exception);
            Assert.IsNotNull(addStorageResult.Data);

            var storageItem2Id = addStorageResult.Data;
            Assert.IsNotNull(storageItem2Id);
            Assert.IsTrue(storageItem2Id.Value != Guid.Empty);

            var task = new ListRecordLabelDigitalMedia(DbContext);
            var result = task.DoTask(testRecordLabel);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);

            var recordLabelDigitalMedia = result.Data;
            Assert.IsNotNull(recordLabelDigitalMedia);
            Assert.IsTrue(recordLabelDigitalMedia.Count >= 2);

            var storageItem1 = recordLabelDigitalMedia.SingleOrDefault(dm => dm.Uuid == storageItem1Id);
            Assert.IsNotNull(storageItem1);
            Assert.AreEqual(testItem1.Name, storageItem1.Name);
            Assert.AreEqual(testItem1.Container, storageItem1.Container);
            Assert.AreEqual(testItem1.FileName, storageItem1.FileName);
            Assert.AreEqual(testItem1.FolderPath, storageItem1.FolderPath);
            Assert.AreEqual(testItem1.IsCompressed, storageItem1.IsCompressed);
            Assert.AreEqual(testItem1.MediaCategory, storageItem1.MediaCategory);
            Assert.IsNotNull(storageItem1.RecordLabel);
            Assert.AreEqual(testRecordLabel.Name, storageItem1.RecordLabel.Name);
            Assert.AreEqual(testRecordLabel.Email, storageItem1.RecordLabel.Email);

            var storageItem2 = recordLabelDigitalMedia.SingleOrDefault(dm => dm.Uuid == storageItem2Id);
            Assert.IsNotNull(storageItem2);
            Assert.AreEqual(testItem2.Name, storageItem2.Name);
            Assert.AreEqual(testItem2.Container, storageItem2.Container);
            Assert.AreEqual(testItem2.FileName, storageItem2.FileName);
            Assert.AreEqual(testItem2.FolderPath, storageItem2.FolderPath);
            Assert.AreEqual(testItem2.IsCompressed, storageItem2.IsCompressed);
            Assert.AreEqual(testItem2.MediaCategory, storageItem2.MediaCategory);
            Assert.IsNotNull(storageItem2.RecordLabel);
            Assert.AreEqual(testRecordLabel.Name, storageItem2.RecordLabel.Name);
            Assert.AreEqual(testRecordLabel.Email, storageItem2.RecordLabel.Email);

            var removeStorageItemTask = new RemoveStorageItem(DbContext);
            var removeStorageItemResult = removeStorageItemTask.DoTask(storageItem1);

            Assert.IsTrue(removeStorageItemResult.Success);
            Assert.IsNull(removeStorageItemResult.Exception);

            removeStorageItemTask = new RemoveStorageItem(DbContext);
            removeStorageItemResult = removeStorageItemTask.DoTask(storageItem2);

            Assert.IsTrue(removeStorageItemResult.Success);
            Assert.IsNull(removeStorageItemResult.Exception);

            var removeRecordLabelTask = new RemoveRecordLabel(DbContext);
            var removeRecordLabelResult = removeRecordLabelTask.DoTask(testRecordLabel);

            Assert.IsTrue(removeRecordLabelResult.Success);
            Assert.IsNull(removeRecordLabelResult.Exception);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new ListRecordLabelDigitalMedia(EmptyDbContext);
            var result = task.DoTask(null);
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
