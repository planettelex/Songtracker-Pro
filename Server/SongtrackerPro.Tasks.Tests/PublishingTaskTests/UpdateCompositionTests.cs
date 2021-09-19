using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.PublishingTasks;

namespace SongtrackerPro.Tasks.Tests.PublishingTaskTests
{
    [TestClass]
    public class UpdateCompositionTests : TestsBase
    {
        public void UpdateCompositionModel(Composition composition)
        {
            var stamp = DateTime.Now.Ticks;
            composition.Title = "Update " + stamp;
            composition.CatalogNumber = "#" + stamp;
            composition.CopyrightedOn = DateTime.Today.AddDays(-10);
            composition.Iswc = "ISWC" + stamp;
        }

        [TestMethod]
        public void TaskSuccessTest()
        {
            var testPublisher = TestsModel.Publisher;
            var addPublisherTask = new AddPublisher(DbContext, new FormattingService());
            var addPublisherResult = addPublisherTask.DoTask(testPublisher);

            Assert.IsTrue(addPublisherResult.Success);
            Assert.IsNull(addPublisherResult.Exception);

            var testComposition = TestsModel.Composition(testPublisher);
            var addCompositionTask = new AddComposition(DbContext);
            var addCompositionResult = addCompositionTask.DoTask(testComposition);

            Assert.IsTrue(addCompositionResult.Success);
            Assert.IsNull(addCompositionResult.Exception);

            var task = new UpdateComposition(DbContext);
            UpdateCompositionModel(testComposition);
            var result = task.DoTask(testComposition);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNull(result.Data);

            var getCompositionTask = new GetComposition(DbContext);
            var composition = getCompositionTask.DoTask(testComposition.Id)?.Data;

            Assert.IsNotNull(composition);
            Assert.AreEqual(testComposition.Title, composition.Title);
            Assert.AreEqual(testComposition.CatalogNumber, composition.CatalogNumber);
            Assert.AreEqual(testComposition.Iswc, composition.Iswc);
            Assert.AreEqual(testComposition.CopyrightedOn, composition.CopyrightedOn);

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
            var task = new UpdateComposition(EmptyDbContext);
            var result = task.DoTask(null);
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
