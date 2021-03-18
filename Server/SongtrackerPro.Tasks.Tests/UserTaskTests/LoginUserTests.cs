using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Enums;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Tasks.PersonTasks;
using SongtrackerPro.Tasks.UserTasks;

namespace SongtrackerPro.Tasks.Tests.UserTaskTests
{
    [TestClass]
    public class LoginUserTests : TestsBase
    {
        [TestMethod]
        public void TaskSuccessTest()
        {
            var testUser = TestModel.User;
            testUser.Type = UserType.LabelAdministrator;
            var addUserTask = new AddUser(DbContext, new AddPerson(DbContext));
            var addUserResult = addUserTask.DoTask(testUser);

            Assert.IsTrue(addUserResult.Success);
            Assert.IsNull(addUserResult.Exception);

            var task = new LoginUser(DbContext);
            var login = new Login
            {
                AuthenticationId = testUser.AuthenticationId,
                AuthenticationToken = DateTime.Now.Ticks.ToString()
            };
            var result = task.DoTask(login);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Data);

            var getUserTask = new GetUser(DbContext);
            if (result.Data != null)
            {
                var user = getUserTask.DoTask(result.Data.Id)?.Data;

                Assert.IsNotNull(user);
                Assert.AreEqual(login.AuthenticationId, user.AuthenticationId);
                Assert.AreEqual(login.AuthenticationToken, user.AuthenticationToken);
                Assert.IsTrue(testUser.LastLogin < user.LastLogin);
            }
            var removeUserTask = new RemoveUser(DbContext);
            var removeUserResult = removeUserTask.DoTask(testUser);

            Assert.IsTrue(removeUserResult.Success);
            Assert.IsNull(removeUserResult.Exception);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new LoginUser(EmptyDbContext);
            var result = task.DoTask(null);
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
