using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.PublishingTasks;

namespace SongtrackerPro.Tasks.Tests.PublishingTaskTests
{
    [TestClass]
    public class UpdatePublisherTests : TestsBase
    {
        public void UpdatePublisherModel(Publisher publisher)
        {
            var stamp = DateTime.Now.Ticks;
            publisher.Name = "Update " + stamp;
            publisher.TaxId = stamp.ToString();
            publisher.Email = $"test@update{stamp}.com";
            publisher.Phone = TestsModel.PhoneNumber;
            publisher.Address = TestsModel.Address;
            publisher.PerformingRightsOrganizationPublisherNumber = new Random().Next(100000, 999999).ToString();
        }

        [TestMethod]
        public void TaskSuccessTest()
        {
            var testPublisher = TestsModel.Publisher;
            var addPublisherTask = new AddPublisher(DbContext, new FormattingService());
            var addPublisherResult = addPublisherTask.DoTask(testPublisher);

            Assert.IsTrue(addPublisherResult.Success);
            Assert.IsNull(addPublisherResult.Exception);

            var task = new UpdatePublisher(DbContext, new FormattingService());
            UpdatePublisherModel(testPublisher);
            var result = task.DoTask(testPublisher);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNull(result.Data);

            var getPublisherTask = new GetPublisher(DbContext);
            var publisher = getPublisherTask.DoTask(testPublisher.Id)?.Data;
            var formattingService = new FormattingService();

            Assert.IsNotNull(publisher);
            Assert.AreEqual(testPublisher.Name, publisher.Name);
            Assert.AreEqual(formattingService.FormatTaxId(testPublisher.TaxId), publisher.TaxId);
            Assert.AreEqual(testPublisher.Email, publisher.Email);
            Assert.AreEqual(formattingService.FormatPhoneNumber(testPublisher.Phone), publisher.Phone);
            Assert.AreEqual(testPublisher.Address.Street, publisher.Address.Street);
            Assert.AreEqual(testPublisher.Address.City, publisher.Address.City);
            Assert.AreEqual(testPublisher.Address.Region, publisher.Address.Region);
            Assert.AreEqual(testPublisher.Address.PostalCode, publisher.Address.PostalCode);
            Assert.AreEqual(testPublisher.Address.Country.Name, publisher.Address.Country.Name);

            var removePublisherTask = new RemovePublisher(DbContext);
            var removePublisherResult = removePublisherTask.DoTask(publisher);

            Assert.IsTrue(removePublisherResult.Success);
            Assert.IsNull(removePublisherResult.Exception);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new UpdatePublisher(EmptyDbContext, new FormattingService());
            var result = task.DoTask(null);
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
