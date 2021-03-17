using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Tasks.RecordLabelTasks;

namespace SongtrackerPro.Tasks.Tests.RecordLabelTaskTests
{
    [TestClass]
    public class ListRecordLabelsTests : TestsBase
    {
        [TestMethod]
        public void TaskSuccessTest()
        {
            var addRecordLabel = new AddRecordLabel(DbContext);
            var testRecordLabel1 = TestModel.RecordLabel;
            var testRecordLabel1Id = addRecordLabel.DoTask(testRecordLabel1);
            Assert.IsTrue(testRecordLabel1Id.Data.HasValue);
            var testRecordLabel2 = TestModel.RecordLabel;
            var testRecordLabel2Id = addRecordLabel.DoTask(testRecordLabel2);
            Assert.IsTrue(testRecordLabel2Id.Data.HasValue);
            
            var task = new ListRecordLabels(DbContext);
            var result = task.DoTask(null);
            
            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);

            var recordLabels = result.Data;
            Assert.IsNotNull(recordLabels);
            Assert.IsTrue(recordLabels.Count >= 2);

            var recordLabel1 = recordLabels.SingleOrDefault(p => p.Id == testRecordLabel1Id.Data.Value);
            Assert.IsNotNull(recordLabel1);
            Assert.AreEqual(testRecordLabel1.Name, recordLabel1.Name);
            Assert.AreEqual(testRecordLabel1.TaxId, recordLabel1.TaxId);
            Assert.AreEqual(testRecordLabel1.Email, recordLabel1.Email);
            Assert.AreEqual(testRecordLabel1.Phone, recordLabel1.Phone);
            Assert.IsNotNull(recordLabel1.Address);
            Assert.AreEqual(testRecordLabel1.Address.Street, recordLabel1.Address.Street);
            Assert.AreEqual(testRecordLabel1.Address.City, recordLabel1.Address.City);
            Assert.AreEqual(testRecordLabel1.Address.Region, recordLabel1.Address.Region);
            Assert.AreEqual(testRecordLabel1.Address.PostalCode, recordLabel1.Address.PostalCode);
            Assert.IsNotNull(recordLabel1.Address.Country);
            Assert.AreEqual(testRecordLabel1.Address.Country.Name, recordLabel1.Address.Country.Name);
            Assert.AreEqual(testRecordLabel1.Address.Country.IsoCode, recordLabel1.Address.Country.IsoCode);

            var recordLabel2 = recordLabels.SingleOrDefault(p => p.Id == testRecordLabel2Id.Data.Value);
            Assert.IsNotNull(recordLabel2);
            Assert.AreEqual(testRecordLabel2.Name, recordLabel2.Name);
            Assert.AreEqual(testRecordLabel2.TaxId, recordLabel2.TaxId);
            Assert.AreEqual(testRecordLabel2.Email, recordLabel2.Email);
            Assert.AreEqual(testRecordLabel2.Phone, recordLabel2.Phone);
            Assert.IsNotNull(recordLabel2.Address);
            Assert.AreEqual(testRecordLabel2.Address.Street, recordLabel2.Address.Street);
            Assert.AreEqual(testRecordLabel2.Address.City, recordLabel2.Address.City);
            Assert.AreEqual(testRecordLabel2.Address.Region, recordLabel2.Address.Region);
            Assert.AreEqual(testRecordLabel2.Address.PostalCode, recordLabel2.Address.PostalCode);
            Assert.IsNotNull(recordLabel2.Address.Country);
            Assert.AreEqual(testRecordLabel2.Address.Country.Name, recordLabel2.Address.Country.Name);
            Assert.AreEqual(testRecordLabel2.Address.Country.IsoCode, recordLabel2.Address.Country.IsoCode);

            var removeRecordLabel = new RemoveRecordLabel(DbContext);
            var removeResult1 = removeRecordLabel.DoTask(recordLabel1);
            var removeResult2 = removeRecordLabel.DoTask(recordLabel2);

            Assert.IsTrue(removeResult1.Success);
            Assert.IsNull(removeResult1.Exception);

            Assert.IsTrue(removeResult2.Success);
            Assert.IsNull(removeResult2.Exception);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new ListRecordLabels(EmptyDbContext);
            var result = task.DoTask(null);
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
