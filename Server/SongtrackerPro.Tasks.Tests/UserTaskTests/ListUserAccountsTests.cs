using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.PersonTasks;
using SongtrackerPro.Tasks.PlatformTasks;
using SongtrackerPro.Tasks.UserTasks;

namespace SongtrackerPro.Tasks.Tests.UserTaskTests
{
    [TestClass]
    public class ListUserAccountTests : TestsBase
    {
        [TestMethod]
        public void TaskSuccessTest()
        {
            var addUserTask = new AddUser(DbContext, new AddPerson(DbContext, new FormattingService()), new FormattingService());
            var testUser = TestsModel.User;
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

            foreach (var paymentPlatform in paymentPlatforms)
            {
                var userAccount = new UserAccount
                {
                    IsPreferred = new Random().Next(0, 2) == 0,
                    Platform = paymentPlatform,
                    User = testUser,
                    Username = "@user-" + new Random().Next(100, 999)
                };
                var addUserAccountTask = new AddUserAccount(DbContext);
                var addUserAccountResult = addUserAccountTask.DoTask(userAccount);

                Assert.IsTrue(addUserAccountResult.Success);
                Assert.IsNull(addUserAccountResult.Exception);
                Assert.IsNotNull(addUserAccountResult.Data);
            }

            var task = new ListUserAccounts(DbContext);
            var result = task.DoTask(testUser);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual(paymentPlatforms.Count, result.Data.Count);

            foreach (var userAccount in result.Data)
            {
                Assert.AreEqual(userAccount.UserId, testUser.Id);
                Assert.IsNotNull(userAccount.Username);
            }
            
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
