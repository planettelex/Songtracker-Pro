using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.PublishingTasks;

namespace SongtrackerPro.Tasks.Tests.PublishingTaskTests
{
    [TestClass]
    public class AddCompositionTests : TestsBase
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

            var testComposition = TestsModel.Composition(testPublisher);
            var task = new AddComposition(DbContext);
            var result = task.DoTask(testComposition);
            
            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Data);

            var compositionId = result.Data;
            Assert.IsNotNull(compositionId);
            Assert.IsTrue(compositionId > 0);

            var getCompositionTask = new GetComposition(DbContext);
            var composition = getCompositionTask.DoTask(compositionId.Value)?.Data;
            var formattingService = new FormattingService();

            Assert.IsNotNull(composition);
            Assert.AreEqual(testComposition.Title, composition.Title);
            Assert.AreEqual(testComposition.CatalogNumber, composition.CatalogNumber);
            Assert.AreEqual(testComposition.CopyrightedOn, composition.CopyrightedOn);
            Assert.AreEqual(testComposition.Iswc, composition.Iswc);
            Assert.IsNotNull(composition.Publisher);
            Assert.AreEqual(testPublisher.Name, composition.Publisher.Name);
            Assert.AreEqual(formattingService.FormatTaxId(testPublisher.TaxId), composition.Publisher.TaxId);
            Assert.AreEqual(testPublisher.Email, composition.Publisher.Email);
            Assert.AreEqual(formattingService.FormatPhoneNumber(testPublisher.Phone), composition.Publisher.Phone);
            Assert.IsNotNull(composition.Publisher.Address);
            Assert.AreEqual(testPublisher.Address.Street, composition.Publisher.Address.Street);
            Assert.AreEqual(testPublisher.Address.City, composition.Publisher.Address.City);
            Assert.AreEqual(testPublisher.Address.Region, composition.Publisher.Address.Region);
            Assert.AreEqual(testPublisher.Address.PostalCode, composition.Publisher.Address.PostalCode);
            Assert.IsNotNull(composition.Publisher.Address.Country);
            Assert.AreEqual(testPublisher.Address.Country.Name, composition.Publisher.Address.Country.Name);
            Assert.AreEqual(testPublisher.Address.Country.IsoCode, composition.Publisher.Address.Country.IsoCode);
            Assert.IsNotNull(composition.Publisher.PerformingRightsOrganization);
            Assert.AreEqual(testPublisher.PerformingRightsOrganization.Name, composition.Publisher.PerformingRightsOrganization.Name);

            var removeCompositionTask = new RemoveComposition(DbContext);
            var removeCompositionResult = removeCompositionTask.DoTask(testComposition);

            Assert.IsTrue(removeCompositionResult.Success);
            Assert.IsNull(removeCompositionResult.Exception);

            var removePublisherTask = new RemovePublisher(DbContext);
            var removePublisherResult = removePublisherTask.DoTask(testPublisher);

            Assert.IsTrue(removePublisherResult.Success);
            Assert.IsNull(removePublisherResult.Exception);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new AddComposition(EmptyDbContext);
            var result = task.DoTask(new Composition());
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
