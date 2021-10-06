using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Enums;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;
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
            user.FirstName = "Jack";
            user.MiddleName = "M.";
            user.LastName = "Hoff";
            user.NameSuffix = ";)";
            user.Name = user.FirstAndLastName;
            user.Phone = TestsModel.PhoneNumber;
            user.Email = $"test@update{stamp}.com";
            user.Address.Street = newAddress.Street;
            user.Address.City = newAddress.City;
            user.Address.Region = newAddress.Region;
            user.Address.PostalCode = newAddress.PostalCode;
            user.TaxId = TestsModel.SocialSecurityNumber;
            user.PerformingRightsOrganizationMemberNumber = new Random().Next(100000, 999999).ToString();
            user.SoundExchangeAccountNumber = new Random().Next(1000000, 9999999).ToString();
        }

        [TestMethod]
        public void TaskSuccessTest()
        {
            var testUser = TestsModel.User;
            testUser.UserType = UserType.SystemUser;
            var addUserTask = new AddUser(DbContext, new FormattingService());
            var addUserResult = addUserTask.DoTask(testUser);

            Assert.IsTrue(addUserResult.Success);
            Assert.IsNull(addUserResult.Exception);

            var task = new UpdateUser(DbContext, new FormattingService());
            UpdateUserModel(testUser);
            var result = task.DoTask(testUser);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNull(result.Data);

            var getUserTask = new GetUser(DbContext);
            var user = getUserTask.DoTask(testUser.Id)?.Data;
            var formattingService = new FormattingService();

            Assert.IsNotNull(user);
            Assert.AreEqual(formattingService.FormatSocialSecurityNumber(testUser.TaxId), user.TaxId);
            Assert.AreEqual(testUser.PerformingRightsOrganizationId, user.PerformingRightsOrganizationId);
            Assert.AreEqual(testUser.PerformingRightsOrganizationMemberNumber, user.PerformingRightsOrganizationMemberNumber);
            Assert.AreEqual(testUser.SoundExchangeAccountNumber, user.SoundExchangeAccountNumber);
            Assert.AreEqual(testUser.PublisherId, user.PublisherId);
            Assert.AreEqual(testUser.RecordLabelId, user.RecordLabelId);
            Assert.AreEqual(testUser.FirstName, user.FirstName);
            Assert.AreEqual(testUser.MiddleName, user.MiddleName);
            Assert.AreEqual(testUser.LastName, user.LastName);
            Assert.AreEqual(testUser.NameSuffix, user.NameSuffix);
            Assert.AreEqual(formattingService.FormatPhoneNumber(testUser.Phone), user.Phone);
            Assert.AreEqual(testUser.Email, user.Email);
            Assert.IsNotNull(user.Address);
            Assert.AreEqual(testUser.Address.Street, user.Address.Street);
            Assert.AreEqual(testUser.Address.City, user.Address.City);
            Assert.AreEqual(testUser.Address.Region, user.Address.Region);
            Assert.AreEqual(testUser.Address.PostalCode, user.Address.PostalCode);
            Assert.IsNotNull(user.Address.Country);
            Assert.AreEqual(testUser.Address.Country.Name, user.Address.Country.Name);
            Assert.AreEqual(testUser.Address.Country.IsoCode, user.Address.Country.IsoCode);

            var removeUserTask = new RemoveUser(DbContext);
            var removeUserResult = removeUserTask.DoTask(user);

            Assert.IsTrue(removeUserResult.Success);
            Assert.IsNull(removeUserResult.Exception);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new UpdateUser(EmptyDbContext, new FormattingService());
            var result = task.DoTask(null);
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
