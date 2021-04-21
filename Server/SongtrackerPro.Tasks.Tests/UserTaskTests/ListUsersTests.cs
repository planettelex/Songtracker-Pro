using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Enums;
using SongtrackerPro.Tasks.PersonTasks;
using SongtrackerPro.Tasks.UserTasks;

namespace SongtrackerPro.Tasks.Tests.UserTaskTests
{
    [TestClass]
    public class ListUsersTests : TestsBase
    {
        [TestMethod]
        public void TaskSuccessTest()
        {
            var addUserTask = new AddUser(DbContext, new AddPerson(DbContext));
            var testArtistUser1 = TestModel.User;
            testArtistUser1.Type = UserType.SystemUser;
            testArtistUser1.Roles = SystemUserRoles.ArtistMember;
            var testArtistUser1Id = addUserTask.DoTask(testArtistUser1);
            Assert.IsTrue(testArtistUser1Id.Data.HasValue);
            addUserTask = new AddUser(DbContext, new AddPerson(DbContext));
            var testArtistUser2 = TestModel.User;
            testArtistUser2.Type = UserType.SystemUser;
            testArtistUser1.Roles = SystemUserRoles.ArtistMember | SystemUserRoles.VisualArtist;
            var testArtistUser2Id = addUserTask.DoTask(testArtistUser2);
            Assert.IsTrue(testArtistUser2Id.Data.HasValue);
            addUserTask = new AddUser(DbContext, new AddPerson(DbContext));
            var testManagerUser = TestModel.User;
            testManagerUser.Roles = SystemUserRoles.ArtistManager;
            testManagerUser.Type = UserType.SystemUser;
            var testManagerUserId = addUserTask.DoTask(testManagerUser);
            Assert.IsTrue(testManagerUserId.Data.HasValue);
            addUserTask = new AddUser(DbContext, new AddPerson(DbContext));
            var testLabelUser = TestModel.User;
            testLabelUser.Type = UserType.LabelAdministrator;
            var testLabelUserId = addUserTask.DoTask(testLabelUser);
            Assert.IsTrue(testLabelUserId.Data.HasValue);
            addUserTask = new AddUser(DbContext, new AddPerson(DbContext));
            var testPublisherUser = TestModel.User;
            testPublisherUser.Type = UserType.PublisherAdministrator;
            var testPublisherUserId = addUserTask.DoTask(testPublisherUser);
            Assert.IsTrue(testPublisherUserId.Data.HasValue);
            addUserTask = new AddUser(DbContext, new AddPerson(DbContext));
            var testAdminUser = TestModel.User;
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
            Assert.AreEqual(testArtistUser1.Type, user1.Type);
            Assert.AreEqual(testArtistUser1.ProfileImageUrl, user1.ProfileImageUrl);
            Assert.AreEqual(testArtistUser1.AuthenticationId, user1.AuthenticationId);
            Assert.AreEqual(testArtistUser1.AuthenticationToken, user1.AuthenticationToken);
            Assert.AreEqual(testArtistUser1.SocialSecurityNumber, user1.SocialSecurityNumber);
            Assert.AreEqual(testArtistUser1.SoundExchangeAccountNumber, user1.SoundExchangeAccountNumber);
            Assert.IsNotNull(user1.Person);
            Assert.AreEqual(testArtistUser1.Person.FirstName, user1.Person.FirstName);
            Assert.AreEqual(testArtistUser1.Person.MiddleName, user1.Person.MiddleName);
            Assert.AreEqual(testArtistUser1.Person.LastName, user1.Person.LastName);
            Assert.AreEqual(testArtistUser1.Person.NameSuffix, user1.Person.NameSuffix);
            Assert.AreEqual(testArtistUser1.Person.Email, user1.Person.Email);
            Assert.AreEqual(testArtistUser1.Person.Phone, user1.Person.Phone);
            Assert.IsNotNull(user1.Person.Address);
            Assert.AreEqual(testArtistUser1.Person.Address.Street, user1.Person.Address.Street);
            Assert.AreEqual(testArtistUser1.Person.Address.City, user1.Person.Address.City);
            Assert.AreEqual(testArtistUser1.Person.Address.Region, user1.Person.Address.Region);
            Assert.AreEqual(testArtistUser1.Person.Address.PostalCode, user1.Person.Address.PostalCode);
            Assert.IsNotNull(user1.Person.Address.Country);
            Assert.AreEqual(testArtistUser1.Person.Address.Country.Name, user1.Person.Address.Country.Name);
            Assert.AreEqual(testArtistUser1.Person.Address.Country.IsoCode, user1.Person.Address.Country.IsoCode);
            Assert.AreEqual(testArtistUser1.PerformingRightsOrganizationId, user1.PerformingRightsOrganizationId);

            var user2 = users.SingleOrDefault(u => u.Id == testPublisherUserId.Data);
            Assert.IsNotNull(user2);
            Assert.AreEqual(testPublisherUser.Type, user2.Type);
            Assert.AreEqual(testPublisherUser.ProfileImageUrl, user2.ProfileImageUrl);
            Assert.AreEqual(testPublisherUser.AuthenticationId, user2.AuthenticationId);
            Assert.AreEqual(testPublisherUser.AuthenticationToken, user2.AuthenticationToken);
            Assert.AreEqual(testPublisherUser.SocialSecurityNumber, user2.SocialSecurityNumber);
            Assert.AreEqual(testPublisherUser.SoundExchangeAccountNumber, user2.SoundExchangeAccountNumber);
            Assert.IsNotNull(user2.Person);
            Assert.AreEqual(testPublisherUser.Person.FirstName, user2.Person.FirstName);
            Assert.AreEqual(testPublisherUser.Person.MiddleName, user2.Person.MiddleName);
            Assert.AreEqual(testPublisherUser.Person.LastName, user2.Person.LastName);
            Assert.AreEqual(testPublisherUser.Person.NameSuffix, user2.Person.NameSuffix);
            Assert.AreEqual(testPublisherUser.Person.Email, user2.Person.Email);
            Assert.AreEqual(testPublisherUser.Person.Phone, user2.Person.Phone);
            Assert.IsNotNull(user2.Person.Address);
            Assert.AreEqual(testPublisherUser.Person.Address.Street, user2.Person.Address.Street);
            Assert.AreEqual(testPublisherUser.Person.Address.City, user2.Person.Address.City);
            Assert.AreEqual(testPublisherUser.Person.Address.Region, user2.Person.Address.Region);
            Assert.AreEqual(testPublisherUser.Person.Address.PostalCode, user2.Person.Address.PostalCode);
            Assert.IsNotNull(user2.Person.Address.Country);
            Assert.AreEqual(testPublisherUser.Person.Address.Country.Name, user2.Person.Address.Country.Name);
            Assert.AreEqual(testPublisherUser.Person.Address.Country.IsoCode, user2.Person.Address.Country.IsoCode);
            Assert.AreEqual(testPublisherUser.PerformingRightsOrganizationId, user2.PerformingRightsOrganizationId);
            Assert.AreEqual(testPublisherUser.PublisherId, user2.PublisherId);

            var user3 = users.SingleOrDefault(u => u.Id == testLabelUserId.Data);
            Assert.IsNotNull(user3);
            Assert.AreEqual(testLabelUser.Type, user3.Type);
            Assert.AreEqual(testLabelUser.ProfileImageUrl, user3.ProfileImageUrl);
            Assert.AreEqual(testLabelUser.AuthenticationId, user3.AuthenticationId);
            Assert.AreEqual(testLabelUser.AuthenticationToken, user3.AuthenticationToken);
            Assert.AreEqual(testLabelUser.SocialSecurityNumber, user3.SocialSecurityNumber);
            Assert.AreEqual(testLabelUser.SoundExchangeAccountNumber, user3.SoundExchangeAccountNumber);
            Assert.IsNotNull(user3.Person);
            Assert.AreEqual(testLabelUser.Person.FirstName, user3.Person.FirstName);
            Assert.AreEqual(testLabelUser.Person.MiddleName, user3.Person.MiddleName);
            Assert.AreEqual(testLabelUser.Person.LastName, user3.Person.LastName);
            Assert.AreEqual(testLabelUser.Person.NameSuffix, user3.Person.NameSuffix);
            Assert.AreEqual(testLabelUser.Person.Email, user3.Person.Email);
            Assert.AreEqual(testLabelUser.Person.Phone, user3.Person.Phone);
            Assert.IsNotNull(user3.Person.Address);
            Assert.AreEqual(testLabelUser.Person.Address.Street, user3.Person.Address.Street);
            Assert.AreEqual(testLabelUser.Person.Address.City, user3.Person.Address.City);
            Assert.AreEqual(testLabelUser.Person.Address.Region, user3.Person.Address.Region);
            Assert.AreEqual(testLabelUser.Person.Address.PostalCode, user3.Person.Address.PostalCode);
            Assert.IsNotNull(user3.Person.Address.Country);
            Assert.AreEqual(testLabelUser.Person.Address.Country.Name, user3.Person.Address.Country.Name);
            Assert.AreEqual(testLabelUser.Person.Address.Country.IsoCode, user3.Person.Address.Country.IsoCode);
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
            Assert.AreEqual(testArtistUser1.Type, user1.Type);
            Assert.AreEqual(testArtistUser1.ProfileImageUrl, user1.ProfileImageUrl);
            Assert.AreEqual(testArtistUser1.AuthenticationId, user1.AuthenticationId);
            Assert.AreEqual(testArtistUser1.AuthenticationToken, user1.AuthenticationToken);
            Assert.AreEqual(testArtistUser1.SocialSecurityNumber, user1.SocialSecurityNumber);
            Assert.AreEqual(testArtistUser1.SoundExchangeAccountNumber, user1.SoundExchangeAccountNumber);
            Assert.IsNotNull(user1.Person);
            Assert.AreEqual(testArtistUser1.Person.FirstName, user1.Person.FirstName);
            Assert.AreEqual(testArtistUser1.Person.MiddleName, user1.Person.MiddleName);
            Assert.AreEqual(testArtistUser1.Person.LastName, user1.Person.LastName);
            Assert.AreEqual(testArtistUser1.Person.NameSuffix, user1.Person.NameSuffix);
            Assert.AreEqual(testArtistUser1.Person.Email, user1.Person.Email);
            Assert.AreEqual(testArtistUser1.Person.Phone, user1.Person.Phone);
            Assert.IsNotNull(user1.Person.Address);
            Assert.AreEqual(testArtistUser1.Person.Address.Street, user1.Person.Address.Street);
            Assert.AreEqual(testArtistUser1.Person.Address.City, user1.Person.Address.City);
            Assert.AreEqual(testArtistUser1.Person.Address.Region, user1.Person.Address.Region);
            Assert.AreEqual(testArtistUser1.Person.Address.PostalCode, user1.Person.Address.PostalCode);
            Assert.IsNotNull(user1.Person.Address.Country);
            Assert.AreEqual(testArtistUser1.Person.Address.Country.Name, user1.Person.Address.Country.Name);
            Assert.AreEqual(testArtistUser1.Person.Address.Country.IsoCode, user1.Person.Address.Country.IsoCode);
            Assert.AreEqual(testArtistUser1.PerformingRightsOrganizationId, user1.PerformingRightsOrganizationId);

            user2 = users.SingleOrDefault(u => u.Id == testArtistUser2Id.Data.Value);
            Assert.IsNotNull(user2);
            Assert.AreEqual(testArtistUser2.Type, user2.Type);
            Assert.AreEqual(testArtistUser2.ProfileImageUrl, user2.ProfileImageUrl);
            Assert.AreEqual(testArtistUser2.AuthenticationId, user2.AuthenticationId);
            Assert.AreEqual(testArtistUser2.AuthenticationToken, user2.AuthenticationToken);
            Assert.AreEqual(testArtistUser2.SocialSecurityNumber, user2.SocialSecurityNumber);
            Assert.AreEqual(testArtistUser2.SoundExchangeAccountNumber, user2.SoundExchangeAccountNumber);
            Assert.IsNotNull(user2.Person);
            Assert.AreEqual(testArtistUser2.Person.FirstName, user2.Person.FirstName);
            Assert.AreEqual(testArtistUser2.Person.MiddleName, user2.Person.MiddleName);
            Assert.AreEqual(testArtistUser2.Person.LastName, user2.Person.LastName);
            Assert.AreEqual(testArtistUser2.Person.NameSuffix, user2.Person.NameSuffix);
            Assert.AreEqual(testArtistUser2.Person.Email, user2.Person.Email);
            Assert.AreEqual(testArtistUser2.Person.Phone, user2.Person.Phone);
            Assert.IsNotNull(user2.Person.Address);
            Assert.AreEqual(testArtistUser2.Person.Address.Street, user2.Person.Address.Street);
            Assert.AreEqual(testArtistUser2.Person.Address.City, user2.Person.Address.City);
            Assert.AreEqual(testArtistUser2.Person.Address.Region, user2.Person.Address.Region);
            Assert.AreEqual(testArtistUser2.Person.Address.PostalCode, user2.Person.Address.PostalCode);
            Assert.IsNotNull(user2.Person.Address.Country);
            Assert.AreEqual(testArtistUser2.Person.Address.Country.Name, user2.Person.Address.Country.Name);
            Assert.AreEqual(testArtistUser2.Person.Address.Country.IsoCode, user2.Person.Address.Country.IsoCode);
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
