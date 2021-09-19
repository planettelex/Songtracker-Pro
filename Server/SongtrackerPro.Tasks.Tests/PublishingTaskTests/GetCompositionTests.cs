using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.PublishingTasks;

namespace SongtrackerPro.Tasks.Tests.PublishingTaskTests
{
    [TestClass]
    public class GetCompositionTests : TestsBase
    {
        [TestMethod]
        public void TaskSuccessTest()
        {
            var addPublisherTask = new AddPublisher(DbContext, new FormattingService());
            var testPublisher = TestsModel.Publisher;
            var testPublisherId = addPublisherTask.DoTask(testPublisher);
            Assert.IsTrue(testPublisherId.Data.HasValue);

            var addCompositionTask = new AddComposition(DbContext);
            var testPublication = TestsModel.Composition(testPublisher);
            var testCompositionId = addCompositionTask.DoTask(testPublication);
            Assert.IsTrue(testCompositionId.Data.HasValue);

            var task = new GetComposition(DbContext);
            var result = task.DoTask(testCompositionId.Data.Value);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);

            var composition = result.Data;
            Assert.IsNotNull(composition);
            Assert.AreEqual(testPublication.Title, composition.Title);
            Assert.AreEqual(testPublication.CatalogNumber, composition.CatalogNumber);
            Assert.AreEqual(testPublication.Iswc, composition.Iswc);
            Assert.AreEqual(testPublication.CopyrightedOn, composition.CopyrightedOn);

            var removeCompositionTask = new RemoveComposition(DbContext);
            var removeCompositionResult = removeCompositionTask.DoTask(composition);

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
            var task = new GetComposition(EmptyDbContext);
            var result = task.DoTask(0);
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
