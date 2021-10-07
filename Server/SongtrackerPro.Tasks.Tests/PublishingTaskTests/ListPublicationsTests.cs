using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.PublishingTasks;

namespace SongtrackerPro.Tasks.Tests.PublishingTaskTests
{
    [TestClass]
    public class ListPublicationsTests : TestsBase
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

            var addPublicationTask = new AddPublication(DbContext);
            var testPublication1 = TestsModel.Publication(testPublisher);
            var testPublication1Id = addPublicationTask.DoTask(testPublication1);
            Assert.IsTrue(testPublication1Id.Data.HasValue);
            addPublicationTask = new AddPublication(DbContext);
            var testPublication2 = TestsModel.Publication(testPublisher);
            var testPublication2Id = addPublicationTask.DoTask(testPublication2);
            Assert.IsTrue(testPublication2Id.Data.HasValue);

            var task = new ListPublications(DbContext);
            var result = task.DoTask(testPublisher);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);

            var publications = result.Data;
            Assert.IsNotNull(publications);
            Assert.IsTrue(publications.Count >= 2);

            var publication1 = publications.SingleOrDefault(p => p.Id == testPublication1Id.Data.Value);
            Assert.IsNotNull(publication1);
            Assert.AreEqual(testPublication1.Title, publication1.Title);
            Assert.AreEqual(testPublication1.CatalogNumber, publication1.CatalogNumber);
            Assert.AreEqual(testPublication1.Isbn, publication1.Isbn);
            Assert.AreEqual(testPublication1.CopyrightedOn, publication1.CopyrightedOn);

            var publication2 = publications.SingleOrDefault(p => p.Id == testPublication2Id.Data.Value);
            Assert.IsNotNull(publication2);
            Assert.AreEqual(testPublication2.Title, publication2.Title);
            Assert.AreEqual(testPublication2.CatalogNumber, publication2.CatalogNumber);
            Assert.AreEqual(testPublication2.Isbn, publication2.Isbn);
            Assert.AreEqual(testPublication2.CopyrightedOn, publication2.CopyrightedOn);

            var removePublicationTask = new RemovePublication(DbContext);
            var removeResult1 = removePublicationTask.DoTask(publication1);
            var removeResult2 = removePublicationTask.DoTask(publication2);

            Assert.IsTrue(removeResult1.Success);
            Assert.IsNull(removeResult1.Exception);

            Assert.IsTrue(removeResult2.Success);
            Assert.IsNull(removeResult2.Exception);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new ListPublications(EmptyDbContext);
            var result = task.DoTask(new Publisher());
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
