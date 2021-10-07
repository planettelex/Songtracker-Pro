using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Enums;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.ArtistTasks;
using SongtrackerPro.Tasks.InstallationTasks;
using SongtrackerPro.Tasks.UserTasks;
using SongtrackerPro.Utilities.Services;
using SongtrackerPro.Utilities.Tests.DummyServices;

namespace SongtrackerPro.Tasks.Tests.UserTaskTests
{
    [TestClass]
    public class AcceptUserInvitationTests : TestsBase
    {
        [TestMethod]
        public void TaskSuccessTest()
        {
            var sendUserInvitationTask = new SendUserInvitation(DbContext, new DummyEmailService(), new HtmlService(), new TokenService(), new GetInstallation(DbContext));
            var testUserInvitation = TestsModel.UserInvitation;
            var sendUserInvitationResult = sendUserInvitationTask.DoTask(testUserInvitation);
            
            Assert.IsTrue(sendUserInvitationResult.Success);
            Assert.IsNull(sendUserInvitationResult.Exception);
            Assert.AreNotEqual(sendUserInvitationResult.Data, Guid.Empty);
            
            var userInvitationId = sendUserInvitationResult.Data;
            var getUserInvitationTask = new GetUserInvitation(DbContext);
            var userInvitation = getUserInvitationTask.DoTask(userInvitationId)?.Data;

            Assert.IsNotNull(userInvitation);
            Assert.AreEqual(testUserInvitation.InvitedByUserId, userInvitation.InvitedByUserId);
            Assert.AreEqual(testUserInvitation.UserType, userInvitation.UserType);
            Assert.AreEqual(testUserInvitation.Email, userInvitation.Email);
            Assert.IsNotNull(userInvitation.SentOn);

            userInvitation.CreatedUser = TestsModel.User;
            userInvitation.CreatedUser.AuthenticationId = userInvitation.CreatedUser.Email = null;
            userInvitation.CreatedUser.UserType = UserType.Unassigned;

            var task = new AcceptUserInvitation(DbContext, new DummyEmailService(), new HtmlService(), 
                new TokenService(), new AddUser(DbContext, new FormattingService()), 
                new AddArtistMember(DbContext), new AddArtistManager(DbContext), 
                new GetInstallation(DbContext));
            var result = task.DoTask(userInvitation);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Data);

            var newUser = result.Data;
            Assert.AreEqual(userInvitation.Email, newUser.AuthenticationId);
            Assert.AreEqual(userInvitation.UserType, newUser.UserType);
            Assert.AreEqual(userInvitation.CreatedUser.FirstName, newUser.FirstName);
            Assert.AreEqual(userInvitation.CreatedUser.MiddleName, newUser.MiddleName);
            Assert.AreEqual(userInvitation.CreatedUser.LastName, newUser.LastName);
            Assert.AreEqual(userInvitation.CreatedUser.NameSuffix, newUser.NameSuffix);
            Assert.AreEqual(userInvitation.CreatedUser.Email, newUser.Email);
            Assert.AreEqual(userInvitation.CreatedUser.Phone, newUser.Phone);
            Assert.AreEqual(userInvitation.CreatedUser.Address.Street, newUser.Address.Street);
            Assert.AreEqual(userInvitation.CreatedUser.Address.City, newUser.Address.City);
            Assert.AreEqual(userInvitation.CreatedUser.Address.Region, newUser.Address.Region);
            Assert.AreEqual(userInvitation.CreatedUser.Address.Country.Name, newUser.Address.Country.Name);
            Assert.AreEqual(userInvitation.CreatedUser.Address.Country.IsoCode, newUser.Address.Country.IsoCode);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new AcceptUserInvitation(EmptyDbContext, new DummyEmailService(), new HtmlService(), 
                new TokenService(),  new AddUser(DbContext, new FormattingService()), 
                new AddArtistMember(EmptyDbContext), new AddArtistManager(EmptyDbContext), 
                new GetInstallation(DbContext));
            var result = task.DoTask(new UserInvitation());
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
