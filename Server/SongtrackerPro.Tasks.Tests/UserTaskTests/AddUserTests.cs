using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Models;
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
            var task = new AddUser(DbContext, new AddPerson(DbContext));
            var testUser = TestModel.User;
            var result = task.DoTask(testUser);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Data);

            var userId = result.Data;
            Assert.IsNotNull(userId);
            Assert.IsTrue(userId > 0);

            var getUserTask = new GetUser(DbContext);
            var user = getUserTask.DoTask(userId.Value)?.Data;

            Assert.IsNotNull(user);
            Assert.AreEqual(testUser.Type, user.Type);
            Assert.AreEqual(testUser.AuthenticationId, user.AuthenticationId);
            Assert.AreEqual(testUser.AuthenticationToken, user.AuthenticationToken);
            Assert.AreEqual(testUser.ProfileImageUrl, user.ProfileImageUrl);
            Assert.AreEqual(testUser.LastLogin, user.LastLogin);
            Assert.IsNotNull(testUser.Person);
            Assert.AreEqual(testUser.Person.FirstName, user.Person.FirstName);
            Assert.AreEqual(testUser.Person.MiddleName, user.Person.MiddleName);
            Assert.AreEqual(testUser.Person.LastName, user.Person.LastName);
            Assert.AreEqual(testUser.Person.NameSuffix, user.Person.NameSuffix);
            Assert.AreEqual(testUser.Person.Email, user.Person.Email);
            Assert.AreEqual(testUser.Person.Phone, user.Person.Phone);
            Assert.IsNotNull(testUser.Person.Address.Country);
            Assert.AreEqual(testUser.Person.Address.Country.Name, user.Person.Address.Country.Name);
            Assert.AreEqual(testUser.Person.Address.Country.IsoCode, user.Person.Address.Country.IsoCode);
            if (testUser.PerformingRightsOrganization != null)
            {
                Assert.AreEqual(testUser.PerformingRightsOrganization.Name, user.PerformingRightsOrganization.Name);
                Assert.IsNotNull(testUser.PerformingRightsOrganization.Country);
                Assert.IsNotNull(testUser.PerformingRightsOrganization.Country.Name);
            }

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
            var task = new AddUser(EmptyDbContext, new AddPerson(EmptyDbContext));
            var result = task.DoTask(new User());
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
