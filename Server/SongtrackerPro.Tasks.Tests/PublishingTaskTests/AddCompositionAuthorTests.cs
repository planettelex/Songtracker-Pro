using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.PersonTasks;
using SongtrackerPro.Tasks.PublishingTasks;

namespace SongtrackerPro.Tasks.Tests.PublishingTaskTests
{
    [TestClass]
    public class AddCompositionAuthorTests : TestsBase
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

            var task = new AddCompositionAuthor(DbContext);
            var result = task.DoTask(compositionAuthor);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Data);

            var listCompositionAuthorsTask = new ListCompositionAuthors(DbContext);
            var listCompositionAuthorsResult = listCompositionAuthorsTask.DoTask(testComposition);

            Assert.IsTrue(listCompositionAuthorsResult.Success);
            Assert.IsNull(listCompositionAuthorsResult.Exception);
            Assert.IsNotNull(listCompositionAuthorsResult.Data);

            var author = listCompositionAuthorsResult.Data.SingleOrDefault(a => a.Id == compositionAuthor.Id);
            Assert.IsNotNull(author);
            Assert.AreEqual(compositionAuthor.OwnershipPercentage, author.OwnershipPercentage);

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
            var task = new AddCompositionAuthor(EmptyDbContext);
            var result = task.DoTask(new CompositionAuthor());
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
