using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.LegalEntityTasks;
using SongtrackerPro.Tasks.PersonTasks;

namespace SongtrackerPro.Tasks.Tests.LegalEntityTaskTests
{
    [TestClass]
    public class ListLegalEntityClientsTests : TestsBase
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
            var testPerson1 = TestsModel.Person;
            var addTestPersonResult1 = addPersonTask.DoTask(testPerson1);

            Assert.IsTrue(addTestPersonResult1.Success);
            Assert.IsNull(addTestPersonResult1.Exception);
            Assert.IsNotNull(addTestPersonResult1.Data);

            var testPerson2 = TestsModel.Person;
            var addTestPersonResult2 = addPersonTask.DoTask(testPerson2);

            Assert.IsTrue(addTestPersonResult2.Success);
            Assert.IsNull(addTestPersonResult2.Exception);
            Assert.IsNotNull(addTestPersonResult2.Data);

            var addLegalEntityClientTask = new AddLegalEntityClient(DbContext);
            var client1 = new LegalEntityClient
            {
                LegalEntity = testLegalEntity,
                Person = testPerson1
            };
            addLegalEntityClientTask.DoTask(client1);
            var client2 = new LegalEntityClient
            {
                LegalEntity = testLegalEntity,
                Person = testPerson2
            };
            addLegalEntityClientTask.DoTask(client2);

            var task = new ListLegalEntityClients(DbContext);
            var result = task.DoTask(testLegalEntity);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Data);
            Assert.IsTrue(result.Data.Count >= 2);

            foreach (var legalEntityClient in result.Data)
                Assert.AreEqual(legalEntityClient.LegalEntityId, testLegalEntity.Id);
            
            var removeLegalEntityTask = new RemoveLegalEntity(DbContext);
            var removeLegalEntityResult = removeLegalEntityTask.DoTask(testLegalEntity);

            Assert.IsTrue(removeLegalEntityResult.Success);
            Assert.IsNull(removeLegalEntityResult.Exception);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new ListLegalEntityClients(EmptyDbContext);
            var result = task.DoTask(new LegalEntity());
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
