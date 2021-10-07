using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.PublishingTasks;

namespace SongtrackerPro.Tasks.Tests.PublishingTaskTests
{
    [TestClass]
    public class ListCompositionsTests : TestsBase
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

            var addCompositionTask = new AddComposition(DbContext);
            var testComposition1 = TestsModel.Composition(testPublisher);
            var testComposition1Id = addCompositionTask.DoTask(testComposition1);
            Assert.IsTrue(testComposition1Id.Data.HasValue);
            addCompositionTask = new AddComposition(DbContext);
            var testComposition2 = TestsModel.Composition(testPublisher);
            var testComposition2Id = addCompositionTask.DoTask(testComposition2);
            Assert.IsTrue(testComposition2Id.Data.HasValue);

            var task = new ListCompositions(DbContext);
            var result = task.DoTask(testPublisher);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);

            var compositions = result.Data;
            Assert.IsNotNull(compositions);
            Assert.IsTrue(compositions.Count >= 2);

            var composition1 = compositions.SingleOrDefault(p => p.Id == testComposition1Id.Data.Value);
            Assert.IsNotNull(composition1);
            Assert.AreEqual(testComposition1.Title, composition1.Title);
            Assert.AreEqual(testComposition1.CatalogNumber, composition1.CatalogNumber);
            Assert.AreEqual(testComposition1.Iswc, composition1.Iswc);
            Assert.AreEqual(testComposition1.CopyrightedOn, composition1.CopyrightedOn);

            var composition2 = compositions.SingleOrDefault(p => p.Id == testComposition2Id.Data.Value);
            Assert.IsNotNull(composition2);
            Assert.AreEqual(testComposition2.Title, composition2.Title);
            Assert.AreEqual(testComposition2.CatalogNumber, composition2.CatalogNumber);
            Assert.AreEqual(testComposition2.Iswc, composition2.Iswc);
            Assert.AreEqual(testComposition2.CopyrightedOn, composition2.CopyrightedOn);

            var removeCompositionTask = new RemoveComposition(DbContext);
            var removeResult1 = removeCompositionTask.DoTask(composition1);
            var removeResult2 = removeCompositionTask.DoTask(composition2);

            Assert.IsTrue(removeResult1.Success);
            Assert.IsNull(removeResult1.Exception);

            Assert.IsTrue(removeResult2.Success);
            Assert.IsNull(removeResult2.Exception);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new ListCompositions(EmptyDbContext);
            var result = task.DoTask(new Publisher());
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
