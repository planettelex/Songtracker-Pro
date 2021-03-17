using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Tasks.RecordLabelTasks;

namespace SongtrackerPro.Tasks.Tests.RecordLabelTasks
{
    [TestClass]
    public class UpdateRecordLabelTests : TestsBase
    {
        public void UpdateRecordLabelModel(RecordLabel recordLabel)
        {
            var stamp = DateTime.Now.Ticks;
            recordLabel.Name = "Update " + stamp;
            recordLabel.TaxId = stamp.ToString();
            recordLabel.Email = $"test@update{stamp}.com";
            recordLabel.Phone = TestModel.PhoneNumber;
            recordLabel.Address = TestModel.Address;
        }

        [TestMethod]
        public void TaskSuccessTest()
        {
            var testRecordLabel = TestModel.RecordLabel;
            var addRecordLabelTask = new AddRecordLabel(DbContext);
            var addRecordLabelResult = addRecordLabelTask.DoTask(testRecordLabel);

            Assert.IsTrue(addRecordLabelResult.Success);
            Assert.IsNull(addRecordLabelResult.Exception);

            var task = new UpdateRecordLabel(DbContext);
            var toUpdate = testRecordLabel;
            UpdateRecordLabelModel(toUpdate);
            var result = task.DoTask(toUpdate);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNull(result.Data);

            var getRecordLabelTask = new GetRecordLabel(DbContext);
            var recordLabel = getRecordLabelTask.DoTask(toUpdate.Id)?.Data;

            Assert.IsNotNull(recordLabel);
            Assert.AreEqual(toUpdate.Name, recordLabel.Name);
            Assert.AreEqual(toUpdate.TaxId, recordLabel.TaxId);
            Assert.AreEqual(toUpdate.Email, recordLabel.Email);
            Assert.AreEqual(toUpdate.Phone, recordLabel.Phone);
            Assert.AreEqual(toUpdate.Address.Street, recordLabel.Address.Street);
            Assert.AreEqual(toUpdate.Address.City, recordLabel.Address.City);
            Assert.AreEqual(toUpdate.Address.Region, recordLabel.Address.Region);
            Assert.AreEqual(toUpdate.Address.PostalCode, recordLabel.Address.PostalCode);
            Assert.AreEqual(toUpdate.Address.Country.Name, recordLabel.Address.Country.Name);

            var removeRecordLabelTask = new RemoveRecordLabel(DbContext);
            var removeRecordLabelResult = removeRecordLabelTask.DoTask(recordLabel);

            Assert.IsTrue(removeRecordLabelResult.Success);
            Assert.IsNull(removeRecordLabelResult.Exception);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new UpdateRecordLabel(EmptyDbContext);
            var result = task.DoTask(null);
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
