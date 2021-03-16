﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Tasks.PublishingTasks;

namespace SongtrackerPro.Tasks.Tests.PublishingTaskTests
{
    [TestClass]
    public class GetPublisherTests : TestsBase
    {
        [TestMethod]
        public void TaskSuccessTest()
        {
            var addPublisherTask = new AddPublisher(DbContext);
            var testPublisher = TestModel.Publisher;
            var testPublisherId = addPublisherTask.DoTask(testPublisher);
            Assert.IsTrue(testPublisherId.Data.HasValue);

            var task = new GetPublisher(DbContext);
            var result = task.DoTask(testPublisherId.Data.Value);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);

            var publisher = result.Data;
            Assert.IsNotNull(publisher);
            Assert.AreEqual(testPublisher.Name, publisher.Name);
            Assert.AreEqual(testPublisher.TaxId, publisher.TaxId);
            Assert.AreEqual(testPublisher.Email, publisher.Email);
            Assert.AreEqual(testPublisher.Phone, publisher.Phone);
            Assert.IsNotNull(publisher.Address);
            Assert.AreEqual(testPublisher.Address.Street, publisher.Address.Street);
            Assert.AreEqual(testPublisher.Address.City, publisher.Address.City);
            Assert.AreEqual(testPublisher.Address.Region, publisher.Address.Region);
            Assert.AreEqual(testPublisher.Address.PostalCode, publisher.Address.PostalCode);
            Assert.IsNotNull(publisher.Address.Country);
            Assert.AreEqual(testPublisher.Address.Country.Name, publisher.Address.Country.Name);

            var removePublisherTask = new RemovePublisher(DbContext);
            var removeResult = removePublisherTask.DoTask(publisher);

            Assert.IsTrue(removeResult.Success);
            Assert.IsNull(removeResult.Exception);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new GetPublisher(EmptyDbContext);
            var result = task.DoTask(0);
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
