using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Tasks.PublishingTasks;

namespace SongtrackerPro.Tasks.Tests.PublishingTaskTests
{
    [TestClass]
    public class AddPublisherTests : TestsBase
    {
        [TestMethod]
        public void TaskSuccessTest()
        {
            var task = new AddPublisher(DbContext);
            var testPublisher = TestModel.Publisher;
            var result = task.DoTask(testPublisher);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Data);

            var publisherId = result.Data;
            Assert.IsNotNull(publisherId);
            Assert.IsTrue(publisherId > 0);

            var getPublisherTask = new GetPublisher(DbContext);
            var publisher = getPublisherTask.DoTask(publisherId.Value)?.Data;

            Assert.IsNotNull(publisher);
            Assert.AreEqual(testPublisher.Name, publisher.Name);
            Assert.AreEqual(testPublisher.TaxId, publisher.TaxId);
            Assert.AreEqual(testPublisher.Email, publisher.Email);
            Assert.AreEqual(testPublisher.Phone, publisher.Phone);
            Assert.IsNotNull(testPublisher.Address);
            Assert.AreEqual(testPublisher.Address.Street, publisher.Address.Street);
            Assert.AreEqual(testPublisher.Address.City, publisher.Address.City);
            Assert.AreEqual(testPublisher.Address.Region, publisher.Address.Region);
            Assert.AreEqual(testPublisher.Address.PostalCode, publisher.Address.PostalCode);
            Assert.IsNotNull(testPublisher.Address.Country);
            Assert.AreEqual(testPublisher.Address.Country.Name, publisher.Address.Country.Name);
            Assert.AreEqual(testPublisher.Address.Country.IsoCode, publisher.Address.Country.IsoCode);

            var removePublisherTask = new RemovePublisher(DbContext);
            var removeResult = removePublisherTask.DoTask(publisher);

            Assert.IsTrue(removeResult.Success);
            Assert.IsNull(removeResult.Exception);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new AddPublisher(EmptyDbContext);
            var result = task.DoTask(new Publisher());
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
