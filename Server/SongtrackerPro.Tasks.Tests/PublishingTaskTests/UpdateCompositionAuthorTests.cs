using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.PersonTasks;
using SongtrackerPro.Tasks.PublishingTasks;

namespace SongtrackerPro.Tasks.Tests.PublishingTaskTests
{
    [TestClass]
    public class UpdateCompositionAuthorTests : TestsBase
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
            var testComposition = TestsModel.Composition(testPublisher);
            var addCompositionResult = addCompositionTask.DoTask(testComposition);
            
            Assert.IsTrue(addCompositionResult.Success);
            Assert.IsNull(addCompositionResult.Exception);
            Assert.IsNotNull(addCompositionResult.Data);

            var compositionId = addCompositionResult.Data;
            Assert.IsNotNull(compositionId);
            Assert.IsTrue(compositionId > 0);

            var addPersonTask = new AddPerson(DbContext, new FormattingService());
            var testPerson = TestsModel.Person;
            var addPersonResult = addPersonTask.DoTask(testPerson);

            Assert.IsTrue(addPersonResult.Success);
            Assert.IsNull(addPersonResult.Exception);
            Assert.IsNotNull(addPersonResult.Data);

            var authorPerson = testPerson;
            var compositionAuthor = new CompositionAuthor
            {
                Composition = testComposition,
                Author = authorPerson,
                OwnershipPercentage = 100
            };

            var addCompositionAuthorTask = new AddCompositionAuthor(DbContext);
            var addCompositionAuthorResult = addCompositionAuthorTask.DoTask(compositionAuthor);

            Assert.IsTrue(addCompositionAuthorResult.Success);
            Assert.IsNull(addCompositionAuthorResult.Exception);
            Assert.IsNotNull(addCompositionAuthorResult.Data);

            var getCompositionAuthorTask = new GetCompositionAuthor(DbContext);
            var getCompositionAuthorResult = getCompositionAuthorTask.DoTask(compositionAuthor.Id);

            Assert.IsTrue(getCompositionAuthorResult.Success);
            Assert.IsNull(getCompositionAuthorResult.Exception);
            Assert.IsNotNull(getCompositionAuthorResult.Data);

            Assert.AreEqual(compositionAuthor.OwnershipPercentage, getCompositionAuthorResult.Data.OwnershipPercentage);

            const int newPercentage = 20;
            compositionAuthor.OwnershipPercentage = newPercentage;

            var task = new UpdateCompositionAuthor(DbContext);
            var result = task.DoTask(compositionAuthor);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);

            getCompositionAuthorTask = new GetCompositionAuthor(DbContext);
            getCompositionAuthorResult = getCompositionAuthorTask.DoTask(compositionAuthor.Id);

            Assert.IsTrue(getCompositionAuthorResult.Success);
            Assert.IsNull(getCompositionAuthorResult.Exception);
            Assert.AreEqual(newPercentage, getCompositionAuthorResult.Data.OwnershipPercentage);

            var removeCompositionTask = new RemoveComposition(DbContext);
            var removeCompositionResult = removeCompositionTask.DoTask(testComposition);

            Assert.IsTrue(removeCompositionResult.Success);
            Assert.IsNull(removeCompositionResult.Exception);

            var removePublisherTask = new RemovePublisher(DbContext);
            var removePublisherResult = removePublisherTask.DoTask(testPublisher);

            Assert.IsTrue(removePublisherResult.Success);
            Assert.IsNull(removePublisherResult.Exception);

            var removePersonTask = new RemovePerson(DbContext);
            var removePersonResult = removePersonTask.DoTask(authorPerson);

            Assert.IsTrue(removePersonResult.Success);
            Assert.IsNull(removePersonResult.Exception);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new UpdateCompositionAuthor(EmptyDbContext);
            var result = task.DoTask(null);
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
