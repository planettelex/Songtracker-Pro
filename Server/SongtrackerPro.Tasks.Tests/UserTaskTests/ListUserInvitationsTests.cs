using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.InstallationTasks;
using SongtrackerPro.Tasks.UserTasks;
using SongtrackerPro.Utilities.Services;
using SongtrackerPro.Utilities.Tests.DummyServices;

namespace SongtrackerPro.Tasks.Tests.UserTaskTests
{
    [TestClass]
    public class ListUserInvitationsTests : TestsBase
    {
        [TestMethod]
        public void TaskSuccessTest()
        {
            var task = new SendUserInvitation(DbContext, new DummyEmailService(), new HtmlService(), new TokenService(), new GetInstallation(DbContext));
            var testUserInvitation = TestsModel.UserInvitation;
            var result = task.DoTask(testUserInvitation);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.AreNotEqual(result.Data, Guid.Empty);

            var listUserInvitationsTask = new ListUserInvitations(DbContext);
            var userInvitations = listUserInvitationsTask.DoTask(null)?.Data;

            Assert.IsNotNull(userInvitations);
            Assert.IsTrue(userInvitations.Count > 0);

            var sentInvitation = userInvitations.FirstOrDefault(i => i.Uuid == testUserInvitation.Uuid);
            Assert.IsNotNull(sentInvitation);
            Assert.AreEqual(testUserInvitation.Name, sentInvitation.Name);
            Assert.AreEqual(testUserInvitation.Email, sentInvitation.Email);
            Assert.IsNotNull(sentInvitation.InvitedByUser);
            Assert.AreEqual(testUserInvitation.InvitedByUser.Name, sentInvitation.InvitedByUser.Name);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new ListUserInvitations(EmptyDbContext);
            var result = task.DoTask(null);
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
