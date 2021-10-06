using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Enums;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.InstallationTasks;
using SongtrackerPro.Tasks.PlatformTasks;
using SongtrackerPro.Tasks.UserTasks;

namespace SongtrackerPro.Tasks.Tests.UserTaskTests
{
    [TestClass]
    public class UpdateUserAccountTests : TestsBase
    {
        [TestMethod]
        public void TaskSuccessTest()
        {
            var testUser = TestsModel.User;
            testUser.UserType = UserType.PublisherAdministrator;
            var addUserTask = new AddUser(DbContext, new FormattingService());
            var addUserResult = addUserTask.DoTask(testUser);

            Assert.IsTrue(addUserResult.Success);
            Assert.IsNull(addUserResult.Exception);

            var paymentService = new ListServices(DbContext).DoTask(ServiceType.Platform).Data
                .SingleOrDefault(s => s.Name.ToLower() == "payment");
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

            var addUserAccountTask = new AddUserAccount(DbContext);
            var addUserAccountResult = addUserAccountTask.DoTask(userAccount);

            Assert.IsTrue(addUserAccountResult.Success);
            Assert.IsNull(addUserAccountResult.Exception);
            Assert.IsNotNull(addUserAccountResult.Data);

            var getUserAccountTask = new GetUserAccount(DbContext);
            var getUserAccountResult = getUserAccountTask.DoTask(userAccount.Id);

            Assert.IsTrue(getUserAccountResult.Success);
            Assert.IsNull(getUserAccountResult.Exception);
            Assert.IsNotNull(getUserAccountResult.Data);

            Assert.AreEqual(userAccount.PlatformId, getUserAccountResult.Data.PlatformId);
            Assert.AreEqual(userAccount.UserId, getUserAccountResult.Data.UserId);
            Assert.AreEqual(userAccount.IsPreferred, getUserAccountResult.Data.IsPreferred);
            Assert.AreEqual(userAccount.Username, getUserAccountResult.Data.Username);

            userAccount.Username = "@update-" + DateTime.Now.Ticks;
            userAccount.IsPreferred = false;

            var task = new UpdateUserAccount(DbContext);
            var result = task.DoTask(userAccount);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);

            getUserAccountTask = new GetUserAccount(DbContext);
            getUserAccountResult = getUserAccountTask.DoTask(userAccount.Id);
            Assert.AreEqual(userAccount.PlatformId, getUserAccountResult.Data.PlatformId);
            Assert.AreEqual(userAccount.UserId, getUserAccountResult.Data.UserId);
            Assert.AreEqual(userAccount.IsPreferred, getUserAccountResult.Data.IsPreferred);
            Assert.AreEqual(userAccount.Username, getUserAccountResult.Data.Username);

            var removeUserTask = new RemoveUser(DbContext);
            var removeUserResult = removeUserTask.DoTask(testUser);

            Assert.IsTrue(removeUserResult.Success);
            Assert.IsNull(removeUserResult.Exception);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new UpdateUserAccount(EmptyDbContext);
            var result = task.DoTask(null);
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
