using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Tasks.PublishingTasks;

namespace SongtrackerPro.Tasks.Tests.PublishingTaskTests
{
    [TestClass]
    public class ListPublishersTests : TestsBase
    {
        [TestMethod]
        public void TaskSuccessTest()
        {
            var addPublisherTask = new AddPublisher(DbContext);
            var testPublisher1 = TestsModel.Publisher;
            var testPublisher1Id = addPublisherTask.DoTask(testPublisher1);
            Assert.IsTrue(testPublisher1Id.Data.HasValue);
            addPublisherTask = new AddPublisher(DbContext);
            var testPublisher2 = TestsModel.Publisher;
            var testPublisher2Id = addPublisherTask.DoTask(testPublisher2);
            Assert.IsTrue(testPublisher2Id.Data.HasValue);
            
            var task = new ListPublishers(DbContext);
            var result = task.DoTask(null);
            
            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);

            var publishers = result.Data;
            Assert.IsNotNull(publishers);
            Assert.IsTrue(publishers.Count >= 2);

            var publisher1 = publishers.SingleOrDefault(p => p.Id == testPublisher1Id.Data.Value);
            Assert.IsNotNull(publisher1);
            Assert.AreEqual(testPublisher1.Name, publisher1.Name);
            Assert.AreEqual(testPublisher1.TaxId, publisher1.TaxId);
            Assert.AreEqual(testPublisher1.Email, publisher1.Email);
            Assert.AreEqual(testPublisher1.Phone, publisher1.Phone);
            Assert.IsNotNull(publisher1.Address);
            Assert.AreEqual(testPublisher1.Address.Street, publisher1.Address.Street);
            Assert.AreEqual(testPublisher1.Address.City, publisher1.Address.City);
            Assert.AreEqual(testPublisher1.Address.Region, publisher1.Address.Region);
            Assert.AreEqual(testPublisher1.Address.PostalCode, publisher1.Address.PostalCode);
            Assert.IsNotNull(publisher1.Address.Country);
            Assert.AreEqual(testPublisher1.Address.Country.Name, publisher1.Address.Country.Name);
            Assert.AreEqual(testPublisher1.Address.Country.IsoCode, publisher1.Address.Country.IsoCode);
            Assert.IsNotNull(testPublisher1.PerformingRightsOrganization.Country);
            Assert.AreEqual(testPublisher1.PerformingRightsOrganization.Country.Name, publisher1.PerformingRightsOrganization.Country.Name);
            Assert.AreEqual(testPublisher1.PerformingRightsOrganization.Country.IsoCode, publisher1.PerformingRightsOrganization.Country.IsoCode);

            var publisher2 = publishers.SingleOrDefault(p => p.Id == testPublisher2Id.Data.Value);
            Assert.IsNotNull(publisher2);
            Assert.AreEqual(testPublisher2.Name, publisher2.Name);
            Assert.AreEqual(testPublisher2.TaxId, publisher2.TaxId);
            Assert.AreEqual(testPublisher2.Email, publisher2.Email);
            Assert.AreEqual(testPublisher2.Phone, publisher2.Phone);
            Assert.IsNotNull(publisher2.Address);
            Assert.AreEqual(testPublisher2.Address.Street, publisher2.Address.Street);
            Assert.AreEqual(testPublisher2.Address.City, publisher2.Address.City);
            Assert.AreEqual(testPublisher2.Address.Region, publisher2.Address.Region);
            Assert.AreEqual(testPublisher2.Address.PostalCode, publisher2.Address.PostalCode);
            Assert.IsNotNull(publisher2.Address.Country);
            Assert.AreEqual(testPublisher2.Address.Country.Name, publisher2.Address.Country.Name);
            Assert.AreEqual(testPublisher2.Address.Country.IsoCode, publisher2.Address.Country.IsoCode);
            Assert.IsNotNull(testPublisher2.PerformingRightsOrganization.Country);
            Assert.AreEqual(testPublisher2.PerformingRightsOrganization.Country.Name, publisher2.PerformingRightsOrganization.Country.Name);
            Assert.AreEqual(testPublisher2.PerformingRightsOrganization.Country.IsoCode, publisher2.PerformingRightsOrganization.Country.IsoCode);

            var removePublisherTask = new RemovePublisher(DbContext);
            var removeResult1 = removePublisherTask.DoTask(publisher1);
            var removeResult2 = removePublisherTask.DoTask(publisher2);

            Assert.IsTrue(removeResult1.Success);
            Assert.IsNull(removeResult1.Exception);

            Assert.IsTrue(removeResult2.Success);
            Assert.IsNull(removeResult2.Exception);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new ListPublishers(EmptyDbContext);
            var result = task.DoTask(null);
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
