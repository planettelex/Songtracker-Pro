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
            var startTime = DateTime.UtcNow;
            var testUser = TestsModel.User;
            testUser.Type = UserType.LabelAdministrator;
            var addUserTask = new AddUser(DbContext, new AddPerson(DbContext));
            var addUserResult = addUserTask.DoTask(testUser);

            Assert.IsTrue(addUserResult.Success);
            Assert.IsNull(addUserResult.Exception);

            var task = new LoginUser(DbContext);
            var login = new Login
            {
                AuthenticationId = testUser.AuthenticationId,
                AuthenticationToken = TestsModel.AuthenticationToken
            };
            var result = task.DoTask(login);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Data);

            if (result.Data != null)
            {
                var user = result.Data.User;

                Assert.IsNotNull(user);
                Assert.AreEqual(login.AuthenticationToken, result.Data.AuthenticationToken);
                Assert.AreEqual(login.AuthenticationId, user.AuthenticationId);
                Assert.IsTrue(login.LoginAt > startTime);
                Assert.IsTrue(login.LoginAt < DateTime.UtcNow);

                Assert.AreEqual(testUser.Type, user.Type);
                Assert.AreEqual(testUser.Roles, user.Roles);
                
                Assert.IsNotNull(user.Person);
                Assert.AreEqual(testUser.Person.FirstAndLastName, user.Person.FirstAndLastName);
                Assert.AreEqual(testUser.Person.MiddleName, user.Person.MiddleName);
                Assert.AreEqual(testUser.Person.NameSuffix, user.Person.NameSuffix);
                Assert.AreEqual(testUser.Person.Email, user.Person.Email);
                Assert.AreEqual(testUser.Person.Phone, user.Person.Phone);
                Assert.IsNotNull(user.Person.Address);
                Assert.AreEqual(testUser.Person.Address.Street, user.Person.Address.Street);
                Assert.AreEqual(testUser.Person.Address.City, user.Person.Address.City);
                Assert.AreEqual(testUser.Person.Address.Region, user.Person.Address.Region);
                Assert.AreEqual(testUser.Person.Address.PostalCode, user.Person.Address.PostalCode);
                Assert.AreEqual(testUser.Person.Address.Street, user.Person.Address.Street);
                Assert.IsNotNull(user.Person.Address.Country);
                Assert.AreEqual(testUser.Person.Address.Country.Name, user.Person.Address.Country.Name);
                Assert.AreEqual(testUser.Person.Address.Country.IsoCode, user.Person.Address.Country.IsoCode);
            }

            var getLoginTask = new GetLogin(DbContext);
            var getLoginResult = getLoginTask.DoTask(login.AuthenticationToken);

            Assert.IsTrue(getLoginResult.Success);
            Assert.IsNull(getLoginResult.Exception);
            Assert.IsNotNull(getLoginResult.Data);

            if (getLoginResult.Data != null)
            {
                var user = getLoginResult.Data.User;

                Assert.IsNotNull(user);
                Assert.AreEqual(login.AuthenticationToken, getLoginResult.Data.AuthenticationToken);
                Assert.AreEqual(login.AuthenticationId, user.AuthenticationId);
                Assert.IsTrue(login.LoginAt > startTime);
                Assert.IsTrue(login.LoginAt < DateTime.UtcNow);

                Assert.AreEqual(testUser.Type, user.Type);
                Assert.AreEqual(testUser.Roles, user.Roles);
                
                Assert.IsNotNull(user.Person);
                Assert.AreEqual(testUser.Person.FirstAndLastName, user.Person.FirstAndLastName);
                Assert.AreEqual(testUser.Person.MiddleName, user.Person.MiddleName);
                Assert.AreEqual(testUser.Person.NameSuffix, user.Person.NameSuffix);
                Assert.AreEqual(testUser.Person.Email, user.Person.Email);
                Assert.AreEqual(testUser.Person.Phone, user.Person.Phone);
                Assert.IsNotNull(user.Person.Address);
                Assert.AreEqual(testUser.Person.Address.Street, user.Person.Address.Street);
                Assert.AreEqual(testUser.Person.Address.City, user.Person.Address.City);
                Assert.AreEqual(testUser.Person.Address.Region, user.Person.Address.Region);
                Assert.AreEqual(testUser.Person.Address.PostalCode, user.Person.Address.PostalCode);
                Assert.AreEqual(testUser.Person.Address.Street, user.Person.Address.Street);
                Assert.IsNotNull(user.Person.Address.Country);
                Assert.AreEqual(testUser.Person.Address.Country.Name, user.Person.Address.Country.Name);
                Assert.AreEqual(testUser.Person.Address.Country.IsoCode, user.Person.Address.Country.IsoCode);
            }

            var person = testUser.Person;
            var removeUserTask = new RemoveUser(DbContext);
            var removeUserResult = removeUserTask.DoTask(testUser);

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
            var task = new LoginUser(EmptyDbContext);
            var result = task.DoTask(null);
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
