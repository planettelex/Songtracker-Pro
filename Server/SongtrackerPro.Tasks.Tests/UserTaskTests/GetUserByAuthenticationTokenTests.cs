using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Tasks.PersonTasks;
using SongtrackerPro.Tasks.UserTasks;

namespace SongtrackerPro.Tasks.Tests.UserTaskTests
{
    [TestClass]
    public class GetUserByAuthenticationTokenTests : TestsBase
    {
        [TestMethod]
        public void TaskSuccessTest()
        {
            var addUserTask = new AddUser(DbContext, new AddPerson(DbContext));
            var testUser = TestsModel.User;
            var testUserId = addUserTask.DoTask(testUser);
            Assert.IsTrue(testUserId.Data.HasValue);

            var getUserTask = new GetUser(DbContext);
            var result = getUserTask.DoTask(testUserId.Data.Value);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);

            var user = result.Data;
            Assert.IsNotNull(user);

            var authenticationToken = TestsModel.AuthenticationToken;
            user.AuthenticationToken = authenticationToken;

            var updateUserAuthenticationTask = new UpdateUserAuthentication(DbContext);
            updateUserAuthenticationTask.DoTask(user);

            var task = new GetUserByAuthenticationToken(DbContext);
            result = task.DoTask(authenticationToken);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);

            user = result.Data;
            Assert.IsNotNull(user);

            Assert.AreEqual(testUser.AuthenticationId, user.AuthenticationId);
            Assert.AreEqual(authenticationToken, user.AuthenticationToken);
            Assert.AreEqual(testUser.Type, user.Type);
            Assert.AreEqual(testUser.ProfileImageUrl, user.ProfileImageUrl);
            Assert.AreEqual(testUser.LastLogin, user.LastLogin);
            Assert.AreEqual(testUser.SocialSecurityNumber, user.SocialSecurityNumber);
            Assert.AreEqual(testUser.SoundExchangeAccountNumber, user.SoundExchangeAccountNumber);
            Assert.IsNotNull(testUser.Person);
            Assert.AreEqual(testUser.Person.FirstName, user.Person.FirstName);
            Assert.AreEqual(testUser.Person.MiddleName, user.Person.MiddleName);
            Assert.AreEqual(testUser.Person.LastName, user.Person.LastName);
            Assert.AreEqual(testUser.Person.NameSuffix, user.Person.NameSuffix);
            Assert.AreEqual(testUser.Person.Email, user.Person.Email);
            Assert.AreEqual(testUser.Person.Phone, user.Person.Phone);
            Assert.IsNotNull(testUser.Person.Address);
            Assert.AreEqual(testUser.Person.Address.Street, user.Person.Address.Street);
            Assert.AreEqual(testUser.Person.Address.City, user.Person.Address.City);
            Assert.AreEqual(testUser.Person.Address.Region, user.Person.Address.Region);
            Assert.AreEqual(testUser.Person.Address.PostalCode, user.Person.Address.PostalCode);
            Assert.IsNotNull(testUser.Person.Address.Country);
            Assert.AreEqual(testUser.Person.Address.Country.Name, user.Person.Address.Country.Name);
            Assert.AreEqual(testUser.Person.Address.Country.IsoCode, user.Person.Address.Country.IsoCode);
            if (testUser.PerformingRightsOrganization != null)
            {
                Assert.AreEqual(testUser.PerformingRightsOrganization.Name, user.PerformingRightsOrganization.Name);
                Assert.IsNotNull(testUser.PerformingRightsOrganization.Country);
                Assert.AreEqual(testUser.PerformingRightsOrganization.Country.Name, user.PerformingRightsOrganization.Country.Name);
            }
            if (testUser.Publisher != null)
            {
                Assert.AreEqual(testUser.Publisher.Name, user.Publisher.Name);
                Assert.AreEqual(testUser.Publisher.TaxId, user.Publisher.TaxId);
                Assert.AreEqual(testUser.Publisher.Email, user.Publisher.Email);
                Assert.AreEqual(testUser.Publisher.Phone, user.Publisher.Phone);
                Assert.IsNotNull(testUser.Publisher.PerformingRightsOrganization);
                Assert.AreEqual(testUser.Publisher.PerformingRightsOrganization.Name, user.Publisher.PerformingRightsOrganization.Name);
                Assert.IsNotNull(testUser.Publisher.PerformingRightsOrganization.Country);
                Assert.AreEqual(testUser.Publisher.PerformingRightsOrganization.Country.Name, user.Publisher.PerformingRightsOrganization.Country.Name);
                Assert.IsNotNull(testUser.Publisher.Address);
                Assert.AreEqual(testUser.Publisher.Address.Street, user.Publisher.Address.Street);
                Assert.AreEqual(testUser.Publisher.Address.City, user.Publisher.Address.City);
                Assert.AreEqual(testUser.Publisher.Address.Region, user.Publisher.Address.Region);
                Assert.AreEqual(testUser.Publisher.Address.PostalCode, user.Publisher.Address.PostalCode);
                Assert.IsNotNull(testUser.Publisher.Address.Country);
                Assert.AreEqual(testUser.Publisher.Address.Country.Name, user.Publisher.Address.Country.Name);
                Assert.AreEqual(testUser.Publisher.Address.Country.IsoCode, user.Publisher.Address.Country.IsoCode);
            }
            if (testUser.RecordLabel != null)
            {
                Assert.AreEqual(testUser.RecordLabel.Name, user.RecordLabel.Name);
                Assert.AreEqual(testUser.RecordLabel.TaxId, user.RecordLabel.TaxId);
                Assert.AreEqual(testUser.RecordLabel.Email, user.RecordLabel.Email);
                Assert.AreEqual(testUser.RecordLabel.Phone, user.RecordLabel.Phone);
                Assert.IsNotNull(testUser.RecordLabel.Address);
                Assert.AreEqual(testUser.RecordLabel.Address.Street, user.RecordLabel.Address.Street);
                Assert.AreEqual(testUser.RecordLabel.Address.City, user.RecordLabel.Address.City);
                Assert.AreEqual(testUser.RecordLabel.Address.Region, user.RecordLabel.Address.Region);
                Assert.AreEqual(testUser.RecordLabel.Address.PostalCode, user.RecordLabel.Address.PostalCode);
                Assert.IsNotNull(testUser.RecordLabel.Address.Country);
                Assert.AreEqual(testUser.RecordLabel.Address.Country.Name, user.RecordLabel.Address.Country.Name);
                Assert.AreEqual(testUser.RecordLabel.Address.Country.IsoCode, user.RecordLabel.Address.Country.IsoCode);
            }

            var person = user.Person;
            var removeUserTask = new RemoveUser(DbContext);
            var removeResult = removeUserTask.DoTask(user);

            Assert.IsTrue(removeResult.Success);
            Assert.IsNull(removeResult.Exception);

            var removePersonTask = new RemovePerson(DbContext);
            var removePersonResult = removePersonTask.DoTask(person);

            Assert.IsTrue(removePersonResult.Success);
            Assert.IsNull(removePersonResult.Exception);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new GetUser(EmptyDbContext);
            var result = task.DoTask(0);
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
