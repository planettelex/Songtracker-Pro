using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.PersonTasks;
using SongtrackerPro.Tasks.UserTasks;

namespace SongtrackerPro.Tasks.Tests.UserTaskTests
{
    [TestClass]
    public class AddUserTests : TestsBase
    {
        [TestMethod]
        public void TaskSuccessTest()
        {
            var task = new AddUser(DbContext, new AddPerson(DbContext, new FormattingService()), new FormattingService());
            var testUser = TestsModel.User;
            var result = task.DoTask(testUser);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Data);

            var userId = result.Data;
            Assert.IsNotNull(userId);
            Assert.IsTrue(userId > 0);

            var getUserTask = new GetUser(DbContext);
            var user = getUserTask.DoTask(userId.Value)?.Data;
            var formattingService = new FormattingService();

            Assert.IsNotNull(user);
            Assert.AreEqual(testUser.Type, user.Type);
            Assert.AreEqual(testUser.Roles, user.Roles);
            Assert.AreEqual(testUser.AuthenticationId, user.AuthenticationId);
            Assert.IsNotNull(testUser.Person);
            Assert.AreEqual(testUser.Person.FirstName, user.Person.FirstName);
            Assert.AreEqual(testUser.Person.MiddleName, user.Person.MiddleName);
            Assert.AreEqual(testUser.Person.LastName, user.Person.LastName);
            Assert.AreEqual(testUser.Person.NameSuffix, user.Person.NameSuffix);
            Assert.AreEqual(testUser.Person.Email, user.Person.Email);
            Assert.AreEqual(formattingService.FormatPhoneNumber(testUser.Person.Phone), user.Person.Phone);
            Assert.IsNotNull(testUser.Person.Address.Country);
            Assert.AreEqual(testUser.Person.Address.Country.Name, user.Person.Address.Country.Name);
            Assert.AreEqual(testUser.Person.Address.Country.IsoCode, user.Person.Address.Country.IsoCode);
            Assert.AreEqual(formattingService.FormatSocialSecurityNumber(testUser.SocialSecurityNumber), user.SocialSecurityNumber);
            if (testUser.PerformingRightsOrganization != null)
            {
                Assert.AreEqual(testUser.PerformingRightsOrganization.Name, user.PerformingRightsOrganization.Name);
                Assert.IsNotNull(testUser.PerformingRightsOrganization.Country);
                Assert.IsNotNull(testUser.PerformingRightsOrganization.Country.Name);
            }
            Assert.AreEqual(testUser.PerformingRightsOrganizationMemberNumber, user.PerformingRightsOrganizationMemberNumber);
            Assert.AreEqual(testUser.SoundExchangeAccountNumber, user.SoundExchangeAccountNumber);

            var person = user.Person;
            var removeUserTask = new RemoveUser(DbContext);
            var removeUserResult = removeUserTask.DoTask(user);

            Assert.IsTrue(removeUserResult.Success);
            Assert.IsNull(removeUserResult.Exception);

            var removePersonTask = new RemovePerson(DbContext);
            var removePersonResult = removePersonTask.DoTask(person);

            Assert.IsTrue(removePersonResult.Success);
            Assert.IsNull(removePersonResult.Exception);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new AddUser(EmptyDbContext, new AddPerson(EmptyDbContext, new FormattingService()), new FormattingService());
            var result = task.DoTask(new User());
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
