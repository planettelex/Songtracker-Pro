using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Tasks.PublishingTasks;

namespace SongtrackerPro.Tasks.Tests.PublishingTaskTests
{
    [TestClass]
    public class UpdatePublisherTest : TestsBase
    {
        public void UpdatePublisherModel(Publisher publisher)
        {
            var stamp = DateTime.Now.Ticks;
            publisher.Name = "Update " + stamp;
            publisher.TaxId = stamp.ToString();
            publisher.Email = $"test@update{stamp}.com";
            publisher.Phone = TestModel.PhoneNumber;
            publisher.Address = TestModel.Address;
            publisher.PerformingRightsOrganizationPublisherNumber = new Random().Next(100000, 999999).ToString();
        }

        [TestMethod]
        public void TaskSuccessTest()
        {
            var testPublisher = TestModel.Publisher;
            var addPublisherTask = new AddPublisher(DbContext);
            var addPublisherResult = addPublisherTask.DoTask(testPublisher);

            Assert.IsTrue(addPublisherResult.Success);
            Assert.IsNull(addPublisherResult.Exception);

            var task = new UpdatePublisher(DbContext);
            var toUpdate = testPublisher;
            UpdatePublisherModel(toUpdate);
            var result = task.DoTask(toUpdate);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNull(result.Data);

            var getPublisherTask = new GetPublisher(DbContext);
            var publisher = getPublisherTask.DoTask(toUpdate.Id)?.Data;

            Assert.IsNotNull(publisher);
            Assert.AreEqual(toUpdate.Name, publisher.Name);
            Assert.AreEqual(toUpdate.TaxId, publisher.TaxId);
            Assert.AreEqual(toUpdate.Email, publisher.Email);
            Assert.AreEqual(toUpdate.Phone, publisher.Phone);
            Assert.AreEqual(toUpdate.Address.Street, publisher.Address.Street);
            Assert.AreEqual(toUpdate.Address.City, publisher.Address.City);
            Assert.AreEqual(toUpdate.Address.Region, publisher.Address.Region);
            Assert.AreEqual(toUpdate.Address.PostalCode, publisher.Address.PostalCode);
            Assert.AreEqual(toUpdate.Address.Country.Name, publisher.Address.Country.Name);

            var removePublisherTask = new RemovePublisher(DbContext);
            var removePublisherResult = removePublisherTask.DoTask(publisher);

            Assert.IsTrue(removePublisherResult.Success);
            Assert.IsNull(removePublisherResult.Exception);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new UpdatePublisher(EmptyDbContext);
            var result = task.DoTask(null);
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
