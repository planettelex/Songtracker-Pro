using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.PersonTasks;
using SongtrackerPro.Tasks.PublishingTasks;

namespace SongtrackerPro.Tasks.Tests.PublishingTaskTests
{
    [TestClass]
    public class UpdatePublicationAuthorTests : TestsBase
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

            var addPublicationAuthorTask = new AddPublicationAuthor(DbContext);
            var addPublicationAuthorResult = addPublicationAuthorTask.DoTask(publicationAuthor);

            Assert.IsTrue(addPublicationAuthorResult.Success);
            Assert.IsNull(addPublicationAuthorResult.Exception);
            Assert.IsNotNull(addPublicationAuthorResult.Data);

            var getPublicationAuthorTask = new GetPublicationAuthor(DbContext);
            var getPublicationAuthorResult = getPublicationAuthorTask.DoTask(publicationAuthor.Id);

            Assert.IsTrue(getPublicationAuthorResult.Success);
            Assert.IsNull(getPublicationAuthorResult.Exception);
            Assert.IsNotNull(getPublicationAuthorResult.Data);

            Assert.AreEqual(publicationAuthor.OwnershipPercentage, getPublicationAuthorResult.Data.OwnershipPercentage);

            const int newPercentage = 20;
            publicationAuthor.OwnershipPercentage = newPercentage;

            var task = new UpdatePublicationAuthor(DbContext);
            var result = task.DoTask(publicationAuthor);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);

            getPublicationAuthorTask = new GetPublicationAuthor(DbContext);
            getPublicationAuthorResult = getPublicationAuthorTask.DoTask(publicationAuthor.Id);

            Assert.IsTrue(getPublicationAuthorResult.Success);
            Assert.IsNull(getPublicationAuthorResult.Exception);
            Assert.AreEqual(newPercentage, getPublicationAuthorResult.Data.OwnershipPercentage);

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
            var task = new UpdatePublicationAuthor(EmptyDbContext);
            var result = task.DoTask(null);
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
