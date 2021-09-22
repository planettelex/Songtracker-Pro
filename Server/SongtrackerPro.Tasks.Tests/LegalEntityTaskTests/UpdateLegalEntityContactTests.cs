using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.LegalEntityTasks;
using SongtrackerPro.Tasks.PersonTasks;

namespace SongtrackerPro.Tasks.Tests.LegalEntityTaskTests
{
    [TestClass]
    public class UpdateLegalEntityContactTests : TestsBase
    {
        [TestMethod]
        public void TaskSuccessTest()
        {
            var addLegalEntityTask = new AddLegalEntity(DbContext, new FormattingService());
            var testLegalEntity = TestsModel.LegalEntity;
            var addLegalEntityResult = addLegalEntityTask.DoTask(testLegalEntity);

            Assert.IsTrue(addLegalEntityResult.Success);
            Assert.IsNull(addLegalEntityResult.Exception);
            Assert.IsNotNull(addLegalEntityResult.Data);

            var legalEntityId = addLegalEntityResult.Data;
            Assert.IsNotNull(legalEntityId);
            Assert.IsTrue(legalEntityId > 0);

            var addPersonTask = new AddPerson(DbContext, new FormattingService());
            var testPerson = TestsModel.Person;
            var addPersonResult = addPersonTask.DoTask(testPerson);

            Assert.IsTrue(addPersonResult.Success);
            Assert.IsNull(addPersonResult.Exception);
            Assert.IsNotNull(addPersonResult.Data);

            const string position = "Potential Lunch Winner";
            var legalEntityContact = new LegalEntityContact
            {
                Person = testPerson,
                LegalEntity = testLegalEntity,
                Position = position
            };

            var addContactTask = new AddLegalEntityContact(DbContext);
            var addContactResult = addContactTask.DoTask(legalEntityContact);

            Assert.IsTrue(addContactResult.Success);
            Assert.IsNull(addContactResult.Exception);
            Assert.IsNotNull(addContactResult.Data);

            var getLegalEntityContactTask = new GetLegalEntityContact(DbContext);
            var getLegalEntityContactResult = getLegalEntityContactTask.DoTask(legalEntityContact.Id);

            Assert.IsTrue(getLegalEntityContactResult.Success);
            Assert.IsNull(getLegalEntityContactResult.Exception);
            Assert.IsNotNull(getLegalEntityContactResult.Data);

            Assert.AreEqual(legalEntityContact.LegalEntityId, getLegalEntityContactResult.Data.LegalEntityId);
            Assert.AreEqual(legalEntityContact.PersonId, getLegalEntityContactResult.Data.PersonId);
            Assert.AreEqual(position, getLegalEntityContactResult.Data.Position);

            const string newPosition = "Sponge Shredder";
            legalEntityContact.Position = newPosition;

            var task = new UpdateLegalEntityContact(DbContext);
            var result = task.DoTask(legalEntityContact);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);

            getLegalEntityContactTask = new GetLegalEntityContact(DbContext);
            getLegalEntityContactResult = getLegalEntityContactTask.DoTask(legalEntityContact.Id);

            Assert.IsTrue(getLegalEntityContactResult.Success);
            Assert.IsNull(getLegalEntityContactResult.Exception);
            Assert.IsNotNull(getLegalEntityContactResult.Data);
            Assert.AreEqual(legalEntityContact.LegalEntityId, getLegalEntityContactResult.Data.LegalEntityId);
            Assert.AreEqual(legalEntityContact.PersonId, getLegalEntityContactResult.Data.PersonId);
            Assert.AreEqual(newPosition, getLegalEntityContactResult.Data.Position);

            var removeLegalEntityTask = new RemoveLegalEntity(DbContext);
            var removeLegalEntityResult = removeLegalEntityTask.DoTask(testLegalEntity);

            Assert.IsTrue(removeLegalEntityResult.Success);
            Assert.IsNull(removeLegalEntityResult.Exception);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new UpdateLegalEntityContact(EmptyDbContext);
            var result = task.DoTask(null);
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
