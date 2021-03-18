﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Tasks.PersonTasks;
using SongtrackerPro.Tasks.PlatformTasks;
using SongtrackerPro.Tasks.UserTasks;

namespace SongtrackerPro.Tasks.Tests.UserTaskTests
{
    [TestClass]
    public class AddUserAccountTests : TestsBase
    {
        [TestMethod]
        public void TaskSuccessTest()
        {
            var addUserTask = new AddUser(DbContext, new AddPerson(DbContext));
            var testUser = TestModel.User;
            var addUserResult = addUserTask.DoTask(testUser);

            Assert.IsTrue(addUserResult.Success);
            Assert.IsNull(addUserResult.Exception);
            Assert.IsNotNull(addUserResult.Data);

            var userId = addUserResult.Data;
            Assert.IsNotNull(userId);
            Assert.IsTrue(userId > 0);

            var paymentService = new ListServices(DbContext).DoTask(null).Data.SingleOrDefault(s => s.Name.ToLower() == "payment");
            Assert.IsNotNull(paymentService);

            var allPlatforms = new ListPlatforms(DbContext).DoTask(null).Data.ToList();
            var paymentPlatforms = new List<Platform>();
            foreach (var platform in allPlatforms)
                paymentPlatforms.AddRange(from service in platform.Services where service.Id == paymentService.Id select platform);

            var paymentPlatform = paymentPlatforms[new Random().Next(0, paymentPlatforms.Count)];

            var userAccount = new UserAccount
            {
                IsPreferred = true,
                Platform = paymentPlatform,
                User = testUser,
                Username = "@user-" + DateTime.Now.Ticks
            };

            var task = new AddUserAccount(DbContext);
            var result = task.DoTask(userAccount);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Data);

            var getUserAccountTask = new GetUserAccount(DbContext);
            var getUserAccountResult = getUserAccountTask.DoTask(userAccount.Id);

            Assert.IsTrue(getUserAccountResult.Success);
            Assert.IsNull(getUserAccountResult.Exception);
            Assert.IsNotNull(getUserAccountResult.Data);

            Assert.AreEqual(userAccount.PlatformId, getUserAccountResult.Data.PlatformId);
            Assert.AreEqual(userAccount.UserId, getUserAccountResult.Data.UserId);
            Assert.AreEqual(userAccount.IsPreferred, getUserAccountResult.Data.IsPreferred);
            Assert.AreEqual(userAccount.Username, getUserAccountResult.Data.Username);

            var removePerson = testUser.Person;
            var removeUserTask = new RemoveUser(DbContext);
            var removeUserResult = removeUserTask.DoTask(testUser);

            Assert.IsTrue(removeUserResult.Success);
            Assert.IsNull(removeUserResult.Exception);

            var removePersonTask = new RemovePerson(DbContext);
            var removePersonResult = removePersonTask.DoTask(removePerson);

            Assert.IsTrue(removePersonResult.Success);
            Assert.IsNull(removePersonResult.Exception);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new AddUserAccount(EmptyDbContext);
            var result = task.DoTask(new UserAccount());
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
