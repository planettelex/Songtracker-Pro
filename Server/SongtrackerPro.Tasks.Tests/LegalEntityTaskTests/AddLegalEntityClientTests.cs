﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.LegalEntityTasks;
using SongtrackerPro.Tasks.PersonTasks;

namespace SongtrackerPro.Tasks.Tests.LegalEntityTaskTests
{
    [TestClass]
    public class AddLegalEntityClientTests : TestsBase
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

            var legalEntityClient = new LegalEntityClient
            {
                Client = testPerson,
                LegalEntity = testLegalEntity
            };

            var task = new AddLegalEntityClient(DbContext);
            var result = task.DoTask(legalEntityClient);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Data);

            var getLegalEntityClientTask = new GetLegalEntityClient(DbContext);
            var getLegalEntityClientResult = getLegalEntityClientTask.DoTask(legalEntityClient.Id);

            Assert.IsTrue(getLegalEntityClientResult.Success);
            Assert.IsNull(getLegalEntityClientResult.Exception);
            Assert.IsNotNull(getLegalEntityClientResult.Data);

            Assert.AreEqual(legalEntityClient.LegalEntityId, getLegalEntityClientResult.Data.LegalEntityId);
            Assert.AreEqual(legalEntityClient.ClientId, getLegalEntityClientResult.Data.ClientId);

            var removeLegalEntityTask = new RemoveLegalEntity(DbContext);
            var removeLegalEntityResult = removeLegalEntityTask.DoTask(testLegalEntity);

            Assert.IsTrue(removeLegalEntityResult.Success);
            Assert.IsNull(removeLegalEntityResult.Exception);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new AddLegalEntityClient(EmptyDbContext);
            var result = task.DoTask(new LegalEntityClient());
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}