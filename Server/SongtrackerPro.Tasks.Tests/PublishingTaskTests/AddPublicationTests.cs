using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.PublishingTasks;

namespace SongtrackerPro.Tasks.Tests.PublishingTaskTests
{
    [TestClass]
    public class AddPublicationTests : TestsBase
    {
        [TestMethod]
        public void TaskSuccessTest()
        {
            var addPublisherTask = new AddPublisher(DbContext, new FormattingService());
            var testPublisher = TestsModel.Publisher;
            var addPublisherResult = addPublisherTask.DoTask(testPublisher);

            Assert.IsTrue(addPublisherResult.Success);
            Assert.IsNull(addPublisherResult.Exception);
            Assert.IsNotNull(addPublisherResult.Data);

            var publisherId = addPublisherResult.Data;
            Assert.IsNotNull(publisherId);
            Assert.IsTrue(publisherId > 0);

            var testPublication = TestsModel.Publication(testPublisher);
            var task = new AddPublication(DbContext);
            var result = task.DoTask(testPublication);
            
            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Data);

            var publicationId = result.Data;
            Assert.IsNotNull(publicationId);
            Assert.IsTrue(publicationId > 0);

            var getPublicationTask = new GetPublication(DbContext);
            var publication = getPublicationTask.DoTask(publicationId.Value)?.Data;
            var formattingService = new FormattingService();

            Assert.IsNotNull(publication);
            Assert.AreEqual(testPublication.Title, publication.Title);
            Assert.AreEqual(testPublication.CatalogNumber, publication.CatalogNumber);
            Assert.AreEqual(testPublication.CopyrightedOn, publication.CopyrightedOn);
            Assert.AreEqual(testPublication.Isbn, publication.Isbn);
            Assert.IsNotNull(publication.Publisher);
            Assert.AreEqual(testPublisher.Name, publication.Publisher.Name);
            Assert.AreEqual(formattingService.FormatTaxId(testPublisher.TaxId), publication.Publisher.TaxId);
            Assert.AreEqual(testPublisher.Email, publication.Publisher.Email);
            Assert.AreEqual(formattingService.FormatPhoneNumber(testPublisher.Phone), publication.Publisher.Phone);
            Assert.IsNotNull(publication.Publisher.Address);
            Assert.AreEqual(testPublisher.Address.Street, publication.Publisher.Address.Street);
            Assert.AreEqual(testPublisher.Address.City, publication.Publisher.Address.City);
            Assert.AreEqual(testPublisher.Address.Region, publication.Publisher.Address.Region);
            Assert.AreEqual(testPublisher.Address.PostalCode, publication.Publisher.Address.PostalCode);
            Assert.IsNotNull(publication.Publisher.Address.Country);
            Assert.AreEqual(testPublisher.Address.Country.Name, publication.Publisher.Address.Country.Name);
            Assert.AreEqual(testPublisher.Address.Country.IsoCode, publication.Publisher.Address.Country.IsoCode);
            Assert.IsNotNull(publication.Publisher.PerformingRightsOrganization);
            Assert.AreEqual(testPublisher.PerformingRightsOrganization.Name, publication.Publisher.PerformingRightsOrganization.Name);

            var removePublicationTask = new RemovePublication(DbContext);
            var removePublicationResult = removePublicationTask.DoTask(testPublication);

            Assert.IsTrue(removePublicationResult.Success);
            Assert.IsNull(removePublicationResult.Exception);

            var removePublisherTask = new RemovePublisher(DbContext);
            var removePublisherResult = removePublisherTask.DoTask(testPublisher);

            Assert.IsTrue(removePublisherResult.Success);
            Assert.IsNull(removePublisherResult.Exception);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new AddPublication(EmptyDbContext);
            var result = task.DoTask(new Publication());
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
