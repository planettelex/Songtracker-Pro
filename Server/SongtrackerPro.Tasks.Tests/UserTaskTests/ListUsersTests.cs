using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Enums;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.UserTasks;

namespace SongtrackerPro.Tasks.Tests.UserTaskTests
{
    [TestClass]
    public class ListUsersTests : TestsBase
    {
        [TestMethod]
        public void TaskSuccessTest()
        {
            var addUserTask = new AddUser(DbContext, new FormattingService());
            var testArtistUser1 = TestsModel.User;
            testArtistUser1.UserType = UserType.SystemUser;
            testArtistUser1.Roles = SystemUserRoles.ArtistMember;
            var testArtistUser1Id = addUserTask.DoTask(testArtistUser1);
            Assert.IsTrue(testArtistUser1Id.Data.HasValue);
            addUserTask = new AddUser(DbContext, new FormattingService());
            var testArtistUser2 = TestsModel.User;
            testArtistUser2.UserType = UserType.SystemUser;
            testArtistUser1.Roles = SystemUserRoles.ArtistMember | SystemUserRoles.VisualArtist;
            var testArtistUser2Id = addUserTask.DoTask(testArtistUser2);
            Assert.IsTrue(testArtistUser2Id.Data.HasValue);
            addUserTask = new AddUser(DbContext, new FormattingService());
            var testManagerUser = TestsModel.User;
            testManagerUser.Roles = SystemUserRoles.ArtistManager;
            testManagerUser.UserType = UserType.SystemUser;
            var testManagerUserId = addUserTask.DoTask(testManagerUser);
            Assert.IsTrue(testManagerUserId.Data.HasValue);
            addUserTask = new AddUser(DbContext, new FormattingService());
            var testLabelUser = TestsModel.User;
            testLabelUser.UserType = UserType.LabelAdministrator;
            var testLabelUserId = addUserTask.DoTask(testLabelUser);
            Assert.IsTrue(testLabelUserId.Data.HasValue);
            addUserTask = new AddUser(DbContext, new FormattingService());
            var testPublisherUser = TestsModel.User;
            testPublisherUser.UserType = UserType.PublisherAdministrator;
            var testPublisherUserId = addUserTask.DoTask(testPublisherUser);
            Assert.IsTrue(testPublisherUserId.Data.HasValue);
            addUserTask = new AddUser(DbContext, new FormattingService());
            var testAdminUser = TestsModel.User;
            var testAdminUserId = addUserTask.DoTask(testAdminUser);
            Assert.IsTrue(testAdminUserId.Data.HasValue);
            
            var task = new ListUsers(DbContext);
            var result = task.DoTask(null);
            
            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);

            var users = result.Data;
            Assert.IsNotNull(users);
            Assert.IsTrue(users.Count >= 6);

            var user1 = users.SingleOrDefault(u => u.Id == testArtistUser1Id.Data.Value);
            Assert.IsNotNull(user1);
            Assert.AreEqual(testArtistUser1.UserType, user1.UserType);
            Assert.AreEqual(testArtistUser1.AuthenticationId, user1.AuthenticationId);
            Assert.AreEqual(testArtistUser1.TaxId, user1.TaxId);
            Assert.AreEqual(testArtistUser1.SoundExchangeAccountNumber, user1.SoundExchangeAccountNumber);
            Assert.AreEqual(testArtistUser1.FirstName, user1.FirstName);
            Assert.AreEqual(testArtistUser1.MiddleName, user1.MiddleName);
            Assert.AreEqual(testArtistUser1.LastName, user1.LastName);
            Assert.AreEqual(testArtistUser1.NameSuffix, user1.NameSuffix);
            Assert.AreEqual(testArtistUser1.Email, user1.Email);
            Assert.AreEqual(testArtistUser1.Phone, user1.Phone);
            Assert.IsNotNull(user1.Address);
            Assert.AreEqual(testArtistUser1.Address.Street, user1.Address.Street);
            Assert.AreEqual(testArtistUser1.Address.City, user1.Address.City);
            Assert.AreEqual(testArtistUser1.Address.Region, user1.Address.Region);
            Assert.AreEqual(testArtistUser1.Address.PostalCode, user1.Address.PostalCode);
            Assert.IsNotNull(user1.Address.Country);
            Assert.AreEqual(testArtistUser1.Address.Country.Name, user1.Address.Country.Name);
            Assert.AreEqual(testArtistUser1.Address.Country.IsoCode, user1.Address.Country.IsoCode);
            Assert.AreEqual(testArtistUser1.PerformingRightsOrganizationId, user1.PerformingRightsOrganizationId);

            var user2 = users.SingleOrDefault(u => u.Id == testPublisherUserId.Data);
            Assert.IsNotNull(user2);
            Assert.AreEqual(testPublisherUser.UserType, user2.UserType);
            Assert.AreEqual(testPublisherUser.AuthenticationId, user2.AuthenticationId);
            Assert.AreEqual(testPublisherUser.TaxId, user2.TaxId);
            Assert.AreEqual(testPublisherUser.SoundExchangeAccountNumber, user2.SoundExchangeAccountNumber);
            Assert.AreEqual(testPublisherUser.FirstName, user2.FirstName);
            Assert.AreEqual(testPublisherUser.MiddleName, user2.MiddleName);
            Assert.AreEqual(testPublisherUser.LastName, user2.LastName);
            Assert.AreEqual(testPublisherUser.NameSuffix, user2.NameSuffix);
            Assert.AreEqual(testPublisherUser.Email, user2.Email);
            Assert.AreEqual(testPublisherUser.Phone, user2.Phone);
            Assert.IsNotNull(user2.Address);
            Assert.AreEqual(testPublisherUser.Address.Street, user2.Address.Street);
            Assert.AreEqual(testPublisherUser.Address.City, user2.Address.City);
            Assert.AreEqual(testPublisherUser.Address.Region, user2.Address.Region);
            Assert.AreEqual(testPublisherUser.Address.PostalCode, user2.Address.PostalCode);
            Assert.IsNotNull(user2.Address.Country);
            Assert.AreEqual(testPublisherUser.Address.Country.Name, user2.Address.Country.Name);
            Assert.AreEqual(testPublisherUser.Address.Country.IsoCode, user2.Address.Country.IsoCode);
            Assert.AreEqual(testPublisherUser.PerformingRightsOrganizationId, user2.PerformingRightsOrganizationId);
            Assert.AreEqual(testPublisherUser.PublisherId, user2.PublisherId);

            var user3 = users.SingleOrDefault(u => u.Id == testLabelUserId.Data);
            Assert.IsNotNull(user3);
            Assert.AreEqual(testLabelUser.UserType, user3.UserType);
            Assert.AreEqual(testLabelUser.AuthenticationId, user3.AuthenticationId);
            Assert.AreEqual(testLabelUser.TaxId, user3.TaxId);
            Assert.AreEqual(testLabelUser.SoundExchangeAccountNumber, user3.SoundExchangeAccountNumber);
            Assert.AreEqual(testLabelUser.FirstName, user3.FirstName);
            Assert.AreEqual(testLabelUser.MiddleName, user3.MiddleName);
            Assert.AreEqual(testLabelUser.LastName, user3.LastName);
            Assert.AreEqual(testLabelUser.NameSuffix, user3.NameSuffix);
            Assert.AreEqual(testLabelUser.Email, user3.Email);
            Assert.AreEqual(testLabelUser.Phone, user3.Phone);
            Assert.IsNotNull(user3.Address);
            Assert.AreEqual(testLabelUser.Address.Street, user3.Address.Street);
            Assert.AreEqual(testLabelUser.Address.City, user3.Address.City);
            Assert.AreEqual(testLabelUser.Address.Region, user3.Address.Region);
            Assert.AreEqual(testLabelUser.Address.PostalCode, user3.Address.PostalCode);
            Assert.IsNotNull(user3.Address.Country);
            Assert.AreEqual(testLabelUser.Address.Country.Name, user3.Address.Country.Name);
            Assert.AreEqual(testLabelUser.Address.Country.IsoCode, user3.Address.Country.IsoCode);
            Assert.AreEqual(testLabelUser.PerformingRightsOrganizationId, user3.PerformingRightsOrganizationId);
            Assert.AreEqual(testLabelUser.RecordLabelId, user3.RecordLabelId);

            result = task.DoTask(UserType.SystemUser);
            
            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);

            users = result.Data;
            Assert.IsNotNull(users);
            Assert.IsTrue(users.Count >= 2);

            user1 = users.SingleOrDefault(u => u.Id == testArtistUser1Id.Data.Value);
            Assert.IsNotNull(user1);
            Assert.AreEqual(testArtistUser1.UserType, user1.UserType);
            Assert.AreEqual(testArtistUser1.AuthenticationId, user1.AuthenticationId);
            Assert.AreEqual(testArtistUser1.TaxId, user1.TaxId);
            Assert.AreEqual(testArtistUser1.SoundExchangeAccountNumber, user1.SoundExchangeAccountNumber);
            Assert.AreEqual(testArtistUser1.FirstName, user1.FirstName);
            Assert.AreEqual(testArtistUser1.MiddleName, user1.MiddleName);
            Assert.AreEqual(testArtistUser1.LastName, user1.LastName);
            Assert.AreEqual(testArtistUser1.NameSuffix, user1.NameSuffix);
            Assert.AreEqual(testArtistUser1.Email, user1.Email);
            Assert.AreEqual(testArtistUser1.Phone, user1.Phone);
            Assert.IsNotNull(user1.Address);
            Assert.AreEqual(testArtistUser1.Address.Street, user1.Address.Street);
            Assert.AreEqual(testArtistUser1.Address.City, user1.Address.City);
            Assert.AreEqual(testArtistUser1.Address.Region, user1.Address.Region);
            Assert.AreEqual(testArtistUser1.Address.PostalCode, user1.Address.PostalCode);
            Assert.IsNotNull(user1.Address.Country);
            Assert.AreEqual(testArtistUser1.Address.Country.Name, user1.Address.Country.Name);
            Assert.AreEqual(testArtistUser1.Address.Country.IsoCode, user1.Address.Country.IsoCode);
            Assert.AreEqual(testArtistUser1.PerformingRightsOrganizationId, user1.PerformingRightsOrganizationId);

            user2 = users.SingleOrDefault(u => u.Id == testArtistUser2Id.Data.Value);
            Assert.IsNotNull(user2);
            Assert.AreEqual(testArtistUser2.UserType, user2.UserType);
            Assert.AreEqual(testArtistUser2.AuthenticationId, user2.AuthenticationId);
            Assert.AreEqual(testArtistUser2.TaxId, user2.TaxId);
            Assert.AreEqual(testArtistUser2.SoundExchangeAccountNumber, user2.SoundExchangeAccountNumber);
            Assert.AreEqual(testArtistUser2.FirstName, user2.FirstName);
            Assert.AreEqual(testArtistUser2.MiddleName, user2.MiddleName);
            Assert.AreEqual(testArtistUser2.LastName, user2.LastName);
            Assert.AreEqual(testArtistUser2.NameSuffix, user2.NameSuffix);
            Assert.AreEqual(testArtistUser2.Email, user2.Email);
            Assert.AreEqual(testArtistUser2.Phone, user2.Phone);
            Assert.IsNotNull(user2.Address);
            Assert.AreEqual(testArtistUser2.Address.Street, user2.Address.Street);
            Assert.AreEqual(testArtistUser2.Address.City, user2.Address.City);
            Assert.AreEqual(testArtistUser2.Address.Region, user2.Address.Region);
            Assert.AreEqual(testArtistUser2.Address.PostalCode, user2.Address.PostalCode);
            Assert.IsNotNull(user2.Address.Country);
            Assert.AreEqual(testArtistUser2.Address.Country.Name, user2.Address.Country.Name);
            Assert.AreEqual(testArtistUser2.Address.Country.IsoCode, user2.Address.Country.IsoCode);
            Assert.AreEqual(testArtistUser2.PerformingRightsOrganizationId, user2.PerformingRightsOrganizationId);
            Assert.AreEqual(testArtistUser2.PublisherId, user2.PublisherId);

            var removeUserTask = new RemoveUser(DbContext);
            var removeResult1 = removeUserTask.DoTask(testArtistUser1);
            var removeResult2 = removeUserTask.DoTask(testArtistUser2);
            var removeResult3 = removeUserTask.DoTask(testManagerUser);
            var removeResult4 = removeUserTask.DoTask(testLabelUser);
            var removeResult5 = removeUserTask.DoTask(testPublisherUser);
            var removeResult6 = removeUserTask.DoTask(testAdminUser);

            Assert.IsTrue(removeResult1.Success);
            Assert.IsNull(removeResult1.Exception);

            Assert.IsTrue(removeResult2.Success);
            Assert.IsNull(removeResult2.Exception);

            Assert.IsTrue(removeResult3.Success);
            Assert.IsNull(removeResult3.Exception);

            Assert.IsTrue(removeResult4.Success);
            Assert.IsNull(removeResult4.Exception);

            Assert.IsTrue(removeResult5.Success);
            Assert.IsNull(removeResult5.Exception);

            Assert.IsTrue(removeResult6.Success);
            Assert.IsNull(removeResult6.Exception);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new ListUsers(EmptyDbContext);
            var result = task.DoTask(null);
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
