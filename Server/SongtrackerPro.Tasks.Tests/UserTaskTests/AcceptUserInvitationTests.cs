using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Enums;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.ArtistTasks;
using SongtrackerPro.Tasks.InstallationTasks;
using SongtrackerPro.Tasks.PersonTasks;
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
            Assert.AreEqual(testUserInvitation.Type, userInvitation.Type);
            Assert.AreEqual(testUserInvitation.Email, userInvitation.Email);
            Assert.IsNotNull(userInvitation.SentOn);

            userInvitation.CreatedUser = TestsModel.User;
            userInvitation.CreatedUser.AuthenticationId = userInvitation.CreatedUser.Person.Email = null;
            userInvitation.CreatedUser.Type = UserType.Unassigned;

            var task = new AcceptUserInvitation(DbContext, new DummyEmailService(), new HtmlService(), new TokenService(), 
                new AddPerson(DbContext), new AddArtistMember(DbContext), new AddArtistManager(DbContext), new GetInstallation(DbContext));
            var result = task.DoTask(userInvitation);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Data);

            var newUser = result.Data;
            Assert.AreEqual(userInvitation.Email, newUser.AuthenticationId);
            Assert.AreEqual(userInvitation.Type, newUser.Type);
            Assert.AreEqual(userInvitation.CreatedUser.Person.FirstName, newUser.Person.FirstName);
            Assert.AreEqual(userInvitation.CreatedUser.Person.MiddleName, newUser.Person.MiddleName);
            Assert.AreEqual(userInvitation.CreatedUser.Person.LastName, newUser.Person.LastName);
            Assert.AreEqual(userInvitation.CreatedUser.Person.NameSuffix, newUser.Person.NameSuffix);
            Assert.AreEqual(userInvitation.CreatedUser.Person.Email, newUser.Person.Email);
            Assert.AreEqual(userInvitation.CreatedUser.Person.Phone, newUser.Person.Phone);
            Assert.AreEqual(userInvitation.CreatedUser.Person.Address.Street, newUser.Person.Address.Street);
            Assert.AreEqual(userInvitation.CreatedUser.Person.Address.City, newUser.Person.Address.City);
            Assert.AreEqual(userInvitation.CreatedUser.Person.Address.Region, newUser.Person.Address.Region);
            Assert.AreEqual(userInvitation.CreatedUser.Person.Address.Country.Name, newUser.Person.Address.Country.Name);
            Assert.AreEqual(userInvitation.CreatedUser.Person.Address.Country.IsoCode, newUser.Person.Address.Country.IsoCode);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new AcceptUserInvitation(EmptyDbContext, new DummyEmailService(), new HtmlService(), new TokenService(), 
                new AddPerson(EmptyDbContext), new AddArtistMember(EmptyDbContext), new AddArtistManager(EmptyDbContext), new GetInstallation(DbContext));
            var result = task.DoTask(new UserInvitation());
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
