using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.PublishingTasks;

namespace SongtrackerPro.Tasks.Tests.PublishingTaskTests
{
    [TestClass]
    public class UpdatePublicationTests : TestsBase
    {
        public void UpdatePublicationModel(Publication publication)
        {
            var stamp = DateTime.Now.Ticks;
            publication.Title = "Update " + stamp;
            publication.CatalogNumber = "#" + stamp;
            publication.CopyrightedOn = DateTime.Today.AddDays(-10);
            publication.Isbn = "ISBN" + stamp;
        }

        [TestMethod]
        public void TaskSuccessTest()
        {
            var testPublisher = TestsModel.Publisher;
            var addPublisherTask = new AddPublisher(DbContext, new FormattingService());
            var addPublisherResult = addPublisherTask.DoTask(testPublisher);

            Assert.IsTrue(addPublisherResult.Success);
            Assert.IsNull(addPublisherResult.Exception);

            var testPublication = TestsModel.Publication(testPublisher);
            var addPublicationTask = new AddPublication(DbContext);
            var addPublicationResult = addPublicationTask.DoTask(testPublication);

            Assert.IsTrue(addPublicationResult.Success);
            Assert.IsNull(addPublicationResult.Exception);

            var task = new UpdatePublication(DbContext);
            UpdatePublicationModel(testPublication);
            var result = task.DoTask(testPublication);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNull(result.Data);

            var getPublicationTask = new GetPublication(DbContext);
            var publication = getPublicationTask.DoTask(testPublication.Id)?.Data;

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
            var task = new UpdatePublication(EmptyDbContext);
            var result = task.DoTask(null);
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
