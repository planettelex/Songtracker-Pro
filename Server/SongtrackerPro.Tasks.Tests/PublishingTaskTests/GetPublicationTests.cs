using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.PublishingTasks;

namespace SongtrackerPro.Tasks.Tests.PublishingTaskTests
{
    [TestClass]
    public class GetPublicationTests : TestsBase
    {
        [TestMethod]
        public void TaskSuccessTest()
        {
            var addPublisherTask = new AddPublisher(DbContext, new FormattingService());
            var testPublisher = TestsModel.Publisher;
            var testPublisherId = addPublisherTask.DoTask(testPublisher);
            Assert.IsTrue(testPublisherId.Data.HasValue);

            var addPublicationTask = new AddPublication(DbContext);
            var testPublication = TestsModel.Publication(testPublisher);
            var testPublicationId = addPublicationTask.DoTask(testPublication);
            Assert.IsTrue(testPublicationId.Data.HasValue);

            var task = new GetPublication(DbContext);
            var result = task.DoTask(testPublicationId.Data.Value);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);

            var publication = result.Data;
            Assert.IsNotNull(publication);
            Assert.AreEqual(testPublication.Title, publication.Title);
            Assert.AreEqual(testPublication.CatalogNumber, publication.CatalogNumber);
            Assert.AreEqual(testPublication.Isbn, publication.Isbn);
            Assert.AreEqual(testPublication.CopyrightedOn, publication.CopyrightedOn);

            var removePublicationTask = new RemovePublication(DbContext);
            var removePublicationResult = removePublicationTask.DoTask(publication);

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
            var task = new GetPublication(EmptyDbContext);
            var result = task.DoTask(0);
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
