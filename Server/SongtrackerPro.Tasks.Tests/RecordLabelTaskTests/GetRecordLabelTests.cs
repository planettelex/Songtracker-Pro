using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Tasks.RecordLabelTasks;

namespace SongtrackerPro.Tasks.Tests.RecordLabelTaskTests
{
    [TestClass]
    public class GetRecordLabelTests : TestsBase
    {
        [TestMethod]
        public void TaskSuccessTest()
        {
            var addRecordLabelTask = new AddRecordLabel(DbContext);
            var testRecordLabel = TestModel.RecordLabel;
            var testRecordLabelId = addRecordLabelTask.DoTask(testRecordLabel);
            Assert.IsTrue(testRecordLabelId.Data.HasValue);

            var task = new GetRecordLabel(DbContext);
            var result = task.DoTask(testRecordLabelId.Data.Value);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);

            var recordLabel = result.Data;
            Assert.IsNotNull(recordLabel);
            Assert.AreEqual(testRecordLabel.Name, recordLabel.Name);
            Assert.AreEqual(testRecordLabel.TaxId, recordLabel.TaxId);
            Assert.AreEqual(testRecordLabel.Email, recordLabel.Email);
            Assert.AreEqual(testRecordLabel.Phone, recordLabel.Phone);
            Assert.IsNotNull(recordLabel.Address);
            Assert.AreEqual(testRecordLabel.Address.Street, recordLabel.Address.Street);
            Assert.AreEqual(testRecordLabel.Address.City, recordLabel.Address.City);
            Assert.AreEqual(testRecordLabel.Address.Region, recordLabel.Address.Region);
            Assert.AreEqual(testRecordLabel.Address.PostalCode, recordLabel.Address.PostalCode);
            Assert.IsNotNull(recordLabel.Address.Country);
            Assert.AreEqual(testRecordLabel.Address.Country.Name, recordLabel.Address.Country.Name);
            Assert.AreEqual(testRecordLabel.Address.Country.IsoCode, recordLabel.Address.Country.IsoCode);

            var removeRecordLabelTask = new RemoveRecordLabel(DbContext);
            var removeResult = removeRecordLabelTask.DoTask(recordLabel);

            Assert.IsTrue(removeResult.Success);
            Assert.IsNull(removeResult.Exception);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new GetRecordLabel(EmptyDbContext);
            var result = task.DoTask(0);
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
