using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.PersonTasks;
using SongtrackerPro.Tasks.PublishingTasks;

namespace SongtrackerPro.Tasks.Tests.PublishingTaskTests
{
    [TestClass]
    public class AddPublicationAuthorTests : TestsBase
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
            var testPublication = TestsModel.Publication(testPublisher);
            var addPublicationResult = addPublicationTask.DoTask(testPublication);
            
            Assert.IsTrue(addPublicationResult.Success);
            Assert.IsNull(addPublicationResult.Exception);
            Assert.IsNotNull(addPublicationResult.Data);

            var publicationId = addPublicationResult.Data;
            Assert.IsNotNull(publicationId);
            Assert.IsTrue(publicationId > 0);

            var addPersonTask = new AddPerson(DbContext, new FormattingService());
            var testPerson = TestsModel.Person;
            var addPersonResult = addPersonTask.DoTask(testPerson);

            Assert.IsTrue(addPersonResult.Success);
            Assert.IsNull(addPersonResult.Exception);
            Assert.IsNotNull(addPersonResult.Data);

            var authorPerson = testPerson;
            var publicationAuthor = new PublicationAuthor
            {
                Publication = testPublication,
                Author = authorPerson,
                OwnershipPercentage = 100
            };

            var task = new AddPublicationAuthor(DbContext);
            var result = task.DoTask(publicationAuthor);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Data);

            var listPublicationAuthorsTask = new ListPublicationAuthors(DbContext);
            var listPublicationAuthorsResult = listPublicationAuthorsTask.DoTask(testPublication);

            Assert.IsTrue(listPublicationAuthorsResult.Success);
            Assert.IsNull(listPublicationAuthorsResult.Exception);
            Assert.IsNotNull(listPublicationAuthorsResult.Data);

            var author = listPublicationAuthorsResult.Data.SingleOrDefault(a => a.Id == publicationAuthor.Id);
            Assert.IsNotNull(author);
            Assert.AreEqual(publicationAuthor.OwnershipPercentage, author.OwnershipPercentage);

            var removePublicationTask = new RemovePublication(DbContext);
            var removePublicationResult = removePublicationTask.DoTask(testPublication);

            Assert.IsTrue(removePublicationResult.Success);
            Assert.IsNull(removePublicationResult.Exception);

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
            var task = new AddPublicationAuthor(EmptyDbContext);
            var result = task.DoTask(new PublicationAuthor());
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
