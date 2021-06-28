using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Enums;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Tasks.PersonTasks;
using SongtrackerPro.Tasks.UserTasks;

namespace SongtrackerPro.Tasks.Tests.UserTaskTests
{
    [TestClass]
    public class UpdateUserTests : TestsBase
    {
        public void UpdateUserModel(User user)
        {
            var stamp = DateTime.Now.Ticks;
            var newAddress = TestsModel.Address;
            user.Person.FirstName = "Jack";
            user.Person.MiddleName = "M.";
            user.Person.LastName = "Hoff";
            user.Person.NameSuffix = ";)";
            user.Person.Phone = TestsModel.PhoneNumber;
            user.Person.Email = $"test@update{stamp}.com";
            user.Person.Address.Street = newAddress.Street;
            user.Person.Address.City = newAddress.City;
            user.Person.Address.Region = newAddress.Region;
            user.Person.Address.PostalCode = newAddress.PostalCode;
            user.SocialSecurityNumber = TestsModel.SocialSecurityNumber;
            user.PerformingRightsOrganizationMemberNumber = new Random().Next(100000, 999999).ToString();
            user.SoundExchangeAccountNumber = new Random().Next(1000000, 9999999).ToString();
        }

        [TestMethod]
        public void TaskSuccessTest()
        {
            var testUser = TestsModel.User;
            testUser.Type = UserType.SystemUser;
            var addUserTask = new AddUser(DbContext, new AddPerson(DbContext));
            var addUserResult = addUserTask.DoTask(testUser);

            Assert.IsTrue(addUserResult.Success);
            Assert.IsNull(addUserResult.Exception);

            var task = new UpdateUser(DbContext, new UpdatePerson(DbContext), new AddPerson(DbContext));
            UpdateUserModel(testUser);
            var result = task.DoTask(testUser);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNull(result.Data);

            var getUserTask = new GetUser(DbContext);
            var user = getUserTask.DoTask(testUser.Id)?.Data;

            Assert.IsNotNull(user);
            Assert.AreEqual(testUser.SocialSecurityNumber, user.SocialSecurityNumber);
            Assert.AreEqual(testUser.PerformingRightsOrganizationId, user.PerformingRightsOrganizationId);
            Assert.AreEqual(testUser.PerformingRightsOrganizationMemberNumber, user.PerformingRightsOrganizationMemberNumber);
            Assert.AreEqual(testUser.SoundExchangeAccountNumber, user.SoundExchangeAccountNumber);
            Assert.AreEqual(testUser.PublisherId, user.PublisherId);
            Assert.AreEqual(testUser.RecordLabelId, user.RecordLabelId);
            Assert.IsNotNull(user.Person);
            Assert.AreEqual(testUser.Person.FirstName, user.Person.FirstName);
            Assert.AreEqual(testUser.Person.MiddleName, user.Person.MiddleName);
            Assert.AreEqual(testUser.Person.LastName, user.Person.LastName);
            Assert.AreEqual(testUser.Person.NameSuffix, user.Person.NameSuffix);
            Assert.AreEqual(testUser.Person.Phone, user.Person.Phone);
            Assert.AreEqual(testUser.Person.Email, user.Person.Email);
            Assert.IsNotNull(user.Person.Address);
            Assert.AreEqual(testUser.Person.Address.Street, user.Person.Address.Street);
            Assert.AreEqual(testUser.Person.Address.City, user.Person.Address.City);
            Assert.AreEqual(testUser.Person.Address.Region, user.Person.Address.Region);
            Assert.AreEqual(testUser.Person.Address.PostalCode, user.Person.Address.PostalCode);
            Assert.IsNotNull(user.Person.Address.Country);
            Assert.AreEqual(testUser.Person.Address.Country.Name, user.Person.Address.Country.Name);
            Assert.AreEqual(testUser.Person.Address.Country.IsoCode, user.Person.Address.Country.IsoCode);

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
            var task = new UpdateUser(EmptyDbContext, new UpdatePerson(EmptyDbContext), new AddPerson(EmptyDbContext));
            var result = task.DoTask(null);
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
