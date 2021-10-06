using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.UserTasks;

namespace SongtrackerPro.Tasks.Tests.UserTaskTests
{
    [TestClass]
    public class AddUserTests : TestsBase
    {
        [TestMethod]
        public void TaskSuccessTest()
        {
            var task = new AddUser(DbContext, new FormattingService());
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
            Assert.AreEqual(testUser.UserType, user.UserType);
            Assert.AreEqual(testUser.Roles, user.Roles);
            Assert.AreEqual(testUser.AuthenticationId, user.AuthenticationId);
            Assert.AreEqual(testUser.FirstName, user.FirstName);
            Assert.AreEqual(testUser.MiddleName, user.MiddleName);
            Assert.AreEqual(testUser.LastName, user.LastName);
            Assert.AreEqual(testUser.NameSuffix, user.NameSuffix);
            Assert.AreEqual(testUser.Email, user.Email);
            Assert.AreEqual(formattingService.FormatPhoneNumber(testUser.Phone), user.Phone);
            Assert.IsNotNull(testUser.Address.Country);
            Assert.AreEqual(testUser.Address.Country.Name, user.Address.Country.Name);
            Assert.AreEqual(testUser.Address.Country.IsoCode, user.Address.Country.IsoCode);
            Assert.AreEqual(formattingService.FormatSocialSecurityNumber(testUser.TaxId), user.TaxId);
            if (testUser.PerformingRightsOrganization != null)
            {
                Assert.AreEqual(testUser.PerformingRightsOrganization.Name, user.PerformingRightsOrganization.Name);
                Assert.IsNotNull(testUser.PerformingRightsOrganization.Country);
                Assert.IsNotNull(testUser.PerformingRightsOrganization.Country.Name);
            }
            Assert.AreEqual(testUser.PerformingRightsOrganizationMemberNumber, user.PerformingRightsOrganizationMemberNumber);
            Assert.AreEqual(testUser.SoundExchangeAccountNumber, user.SoundExchangeAccountNumber);

            var removeUserTask = new RemoveUser(DbContext);
            var removeUserResult = removeUserTask.DoTask(user);

            Assert.IsTrue(removeUserResult.Success);
            Assert.IsNull(removeUserResult.Exception);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new AddUser(EmptyDbContext, new FormattingService());
            var result = task.DoTask(new User());
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
