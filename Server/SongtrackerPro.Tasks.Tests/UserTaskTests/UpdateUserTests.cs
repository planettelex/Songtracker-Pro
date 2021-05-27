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
            var toUpdate = testUser;
            UpdateUserModel(toUpdate);
            var result = task.DoTask(toUpdate);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNull(result.Data);

            var getUserTask = new GetUser(DbContext);
            var user = getUserTask.DoTask(toUpdate.Id)?.Data;

            Assert.IsNotNull(user);
            Assert.AreEqual(toUpdate.SocialSecurityNumber, user.SocialSecurityNumber);
            Assert.AreEqual(toUpdate.PerformingRightsOrganizationId, user.PerformingRightsOrganizationId);
            Assert.AreEqual(toUpdate.PerformingRightsOrganizationMemberNumber, user.PerformingRightsOrganizationMemberNumber);
            Assert.AreEqual(toUpdate.SoundExchangeAccountNumber, user.SoundExchangeAccountNumber);
            Assert.AreEqual(toUpdate.PublisherId, user.PublisherId);
            Assert.AreEqual(toUpdate.RecordLabelId, user.RecordLabelId);
            Assert.IsNotNull(user.Person);
            Assert.AreEqual(toUpdate.Person.FirstName, user.Person.FirstName);
            Assert.AreEqual(toUpdate.Person.MiddleName, user.Person.MiddleName);
            Assert.AreEqual(toUpdate.Person.LastName, user.Person.LastName);
            Assert.AreEqual(toUpdate.Person.NameSuffix, user.Person.NameSuffix);
            Assert.AreEqual(toUpdate.Person.Phone, user.Person.Phone);
            Assert.AreEqual(toUpdate.Person.Email, user.Person.Email);
            Assert.IsNotNull(user.Person.Address);
            Assert.AreEqual(toUpdate.Person.Address.Street, user.Person.Address.Street);
            Assert.AreEqual(toUpdate.Person.Address.City, user.Person.Address.City);
            Assert.AreEqual(toUpdate.Person.Address.Region, user.Person.Address.Region);
            Assert.AreEqual(toUpdate.Person.Address.PostalCode, user.Person.Address.PostalCode);
            Assert.IsNotNull(user.Person.Address.Country);
            Assert.AreEqual(toUpdate.Person.Address.Country.Name, user.Person.Address.Country.Name);
            Assert.AreEqual(toUpdate.Person.Address.Country.IsoCode, user.Person.Address.Country.IsoCode);

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
