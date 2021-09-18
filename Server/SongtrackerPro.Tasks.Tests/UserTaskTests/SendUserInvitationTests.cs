using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.ArtistTasks;
using SongtrackerPro.Tasks.InstallationTasks;
using SongtrackerPro.Tasks.PublishingTasks;
using SongtrackerPro.Tasks.RecordLabelTasks;
using SongtrackerPro.Tasks.UserTasks;
using SongtrackerPro.Utilities.Services;
using SongtrackerPro.Utilities.Tests.DummyServices;

namespace SongtrackerPro.Tasks.Tests.UserTaskTests
{
    [TestClass]
    public class SendUserInvitationTests : TestsBase
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

            var userInvitationId = result.Data;
            var getUserInvitationTask = new GetUserInvitation(DbContext);
            var userInvitation = getUserInvitationTask.DoTask(userInvitationId)?.Data;

            Assert.IsNotNull(userInvitation);
            Assert.AreEqual(testUserInvitation.InvitedByUserId, userInvitation.InvitedByUserId);
            Assert.AreEqual(testUserInvitation.Type, userInvitation.Type);
            Assert.AreEqual(testUserInvitation.Email, userInvitation.Email);
            Assert.IsNotNull(userInvitation.SentOn);

            if (userInvitation.Publisher != null)
            {
                Assert.AreEqual(testUserInvitation.Publisher.Name, userInvitation.Publisher.Name);
                Assert.IsNotNull(userInvitation.Publisher.Address);
                Assert.AreEqual(testUserInvitation.Publisher.Address.Street, userInvitation.Publisher.Address.Street);
                Assert.AreEqual(testUserInvitation.Publisher.Address.City, userInvitation.Publisher.Address.City);
                Assert.AreEqual(testUserInvitation.Publisher.Address.Region, userInvitation.Publisher.Address.Region);
                Assert.AreEqual(testUserInvitation.Publisher.Address.PostalCode, userInvitation.Publisher.Address.PostalCode);
                Assert.IsNotNull(userInvitation.Publisher.Address.Country);
                Assert.AreEqual(testUserInvitation.Publisher.Address.Country.Name, userInvitation.Publisher.Address.Country.Name);
                Assert.AreEqual(testUserInvitation.Publisher.Address.Country.IsoCode, userInvitation.Publisher.Address.Country.IsoCode);
            }

            if (userInvitation.RecordLabel != null)
            {
                Assert.AreEqual(testUserInvitation.RecordLabel.Name, userInvitation.RecordLabel.Name);
                Assert.IsNotNull(userInvitation.RecordLabel.Address);
                Assert.AreEqual(testUserInvitation.RecordLabel.Address.Street, userInvitation.RecordLabel.Address.Street);
                Assert.AreEqual(testUserInvitation.RecordLabel.Address.City, userInvitation.RecordLabel.Address.City);
                Assert.AreEqual(testUserInvitation.RecordLabel.Address.Region, userInvitation.RecordLabel.Address.Region);
                Assert.AreEqual(testUserInvitation.RecordLabel.Address.PostalCode, userInvitation.RecordLabel.Address.PostalCode);
                Assert.IsNotNull(userInvitation.RecordLabel.Address.Country);
                Assert.AreEqual(testUserInvitation.RecordLabel.Address.Country.Name, userInvitation.RecordLabel.Address.Country.Name);
                Assert.AreEqual(testUserInvitation.RecordLabel.Address.Country.IsoCode, userInvitation.RecordLabel.Address.Country.IsoCode);
            }

            if (userInvitation.Artist != null)
            {
                Assert.AreEqual(testUserInvitation.Artist.Name, userInvitation.Artist.Name);
                Assert.AreEqual(testUserInvitation.Artist.TaxId, userInvitation.Artist.TaxId);
                Assert.AreEqual(testUserInvitation.Artist.HasServiceMark, userInvitation.Artist.HasServiceMark);
                Assert.AreEqual(testUserInvitation.Artist.WebsiteUrl, userInvitation.Artist.WebsiteUrl);
                Assert.AreEqual(testUserInvitation.Artist.PressKitUrl, userInvitation.Artist.PressKitUrl);
            }

            var resendUserInvitationTask = new ResendUserInvitation(DbContext, new DummyEmailService(), new HtmlService(), new TokenService(), new GetInstallation(DbContext));
            var resendResults = resendUserInvitationTask.DoTask(testUserInvitation.Uuid);
            Assert.IsTrue(resendResults.Success);
            var resentInvitation = resendResults.Data;
            Assert.IsTrue(resentInvitation.SentOn > testUserInvitation.SentOn);

            var publisher = testUserInvitation.Publisher;
            var recordLabel = testUserInvitation.RecordLabel;
            var artist = testUserInvitation.Artist;
            var invitedByUser = testUserInvitation.InvitedByUser;

            var removeUserInvitationTask = new RemoveUserInvitation(DbContext);
            var removed = removeUserInvitationTask.DoTask(testUserInvitation.Uuid).Success;
            Assert.IsTrue(removed);

            if (invitedByUser.AuthenticationId.StartsWith("test"))
            {
                var removeUserTask = new RemoveUser(DbContext);
                removed = removeUserTask.DoTask(invitedByUser).Success;
                Assert.IsTrue(removed);
            }

            if (publisher.Name.StartsWith(nameof(Publisher)))
            {
                var removePublisherTask = new RemovePublisher(DbContext);
                removed = removePublisherTask.DoTask(publisher).Success;
                Assert.IsTrue(removed);
            }

            if (artist.Name.StartsWith(nameof(Artist)))
            {
                var removeArtistTask = new RemoveArtist(DbContext);
                removed = removeArtistTask.DoTask(artist).Success;
                Assert.IsTrue(removed);
            }

            if (recordLabel.Name.StartsWith(nameof(RecordLabel)))
            {
                var removeLabelTask = new RemoveRecordLabel(DbContext);
                removed = removeLabelTask.DoTask(recordLabel).Success;
                Assert.IsTrue(removed);
            }
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new SendUserInvitation(EmptyDbContext, new DummyEmailService(), new HtmlService(), new TokenService(), new GetInstallation(EmptyDbContext));
            var result = task.DoTask(new UserInvitation());
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
