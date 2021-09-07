using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.PublishingTasks;

namespace SongtrackerPro.Tasks.Tests.PublishingTaskTests
{
    [TestClass]
    public class AddPublisherTests : TestsBase
    {
        [TestMethod]
        public void TaskSuccessTest()
        {
            var task = new AddPublisher(DbContext, new FormattingService());
            var testPublisher = TestsModel.Publisher;
            var result = task.DoTask(testPublisher);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Data);

            var publisherId = result.Data;
            Assert.IsNotNull(publisherId);
            Assert.IsTrue(publisherId > 0);

            var getPublisherTask = new GetPublisher(DbContext);
            var publisher = getPublisherTask.DoTask(publisherId.Value)?.Data;
            var formattingService = new FormattingService();

            Assert.IsNotNull(publisher);
            Assert.AreEqual(testPublisher.Name, publisher.Name);
            Assert.AreEqual(formattingService.FormatTaxId(testPublisher.TaxId), publisher.TaxId);
            Assert.AreEqual(testPublisher.Email, publisher.Email);
            Assert.AreEqual(formattingService.FormatPhoneNumber(testPublisher.Phone), publisher.Phone);
            Assert.IsNotNull(publisher.Address);
            Assert.AreEqual(testPublisher.Address.Street, publisher.Address.Street);
            Assert.AreEqual(testPublisher.Address.City, publisher.Address.City);
            Assert.AreEqual(testPublisher.Address.Region, publisher.Address.Region);
            Assert.AreEqual(testPublisher.Address.PostalCode, publisher.Address.PostalCode);
            Assert.IsNotNull(publisher.Address.Country);
            Assert.AreEqual(testPublisher.Address.Country.Name, publisher.Address.Country.Name);
            Assert.AreEqual(testPublisher.Address.Country.IsoCode, publisher.Address.Country.IsoCode);
            Assert.IsNotNull(publisher.PerformingRightsOrganization);
            Assert.AreEqual(testPublisher.PerformingRightsOrganization.Name, publisher.PerformingRightsOrganization.Name);

            var removePublisherTask = new RemovePublisher(DbContext);
            var removeResult = removePublisherTask.DoTask(publisher);

            Assert.IsTrue(removeResult.Success);
            Assert.IsNull(removeResult.Exception);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new AddPublisher(EmptyDbContext, new FormattingService());
            var result = task.DoTask(new Publisher());
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
