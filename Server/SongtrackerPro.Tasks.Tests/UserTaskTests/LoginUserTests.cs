using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Enums;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.GeographicTasks;
using SongtrackerPro.Tasks.InstallationTasks;
using SongtrackerPro.Tasks.MerchandiseTasks;
using SongtrackerPro.Tasks.PlatformTasks;
using SongtrackerPro.Tasks.PublishingTasks;
using SongtrackerPro.Tasks.RecordLabelTasks;
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
            testUser.UserType = UserType.LabelAdministrator;
            var addUserTask = new AddUser(DbContext, new FormattingService());
            var addUserResult = addUserTask.DoTask(testUser);

            Assert.IsTrue(addUserResult.Success);
            Assert.IsNull(addUserResult.Exception);

            var task = new LoginUser(DbContext, new GetInstallation(DbContext), 
                new SeedSystemData(new SeedInstallation(DbContext), 
                                   new SeedCountries(DbContext), 
                                   new SeedPerformingRightsOrganizations(DbContext, new SeedCountries(DbContext)), 
                                   new SeedServices(DbContext), 
                                   new SeedPlatforms(DbContext, new ListServices(DbContext), new AddPlatform(DbContext)),
                                   new SeedGenres(DbContext),
                                   new SeedRecordingRoles(DbContext),
                                   new SeedMerchandiseCategories(DbContext)));

            var login = new Login
            {
                AuthenticationId = testUser.AuthenticationId,
                AuthenticationToken = TestsModel.AuthenticationToken,
                TokenExpiration = DateTime.UtcNow.AddHours(1)
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

                Assert.AreEqual(testUser.UserType, user.UserType);
                Assert.AreEqual(testUser.Roles, user.Roles);
                
                Assert.IsNotNull(user);
                Assert.AreEqual(testUser.FirstAndLastName, user.FirstAndLastName);
                Assert.AreEqual(testUser.MiddleName, user.MiddleName);
                Assert.AreEqual(testUser.NameSuffix, user.NameSuffix);
                Assert.AreEqual(testUser.Email, user.Email);
                Assert.AreEqual(testUser.Phone, user.Phone);
                Assert.IsNotNull(user.Address);
                Assert.AreEqual(testUser.Address.Street, user.Address.Street);
                Assert.AreEqual(testUser.Address.City, user.Address.City);
                Assert.AreEqual(testUser.Address.Region, user.Address.Region);
                Assert.AreEqual(testUser.Address.PostalCode, user.Address.PostalCode);
                Assert.AreEqual(testUser.Address.Street, user.Address.Street);
                Assert.IsNotNull(user.Address.Country);
                Assert.AreEqual(testUser.Address.Country.Name, user.Address.Country.Name);
                Assert.AreEqual(testUser.Address.Country.IsoCode, user.Address.Country.IsoCode);
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

                Assert.AreEqual(testUser.UserType, user.UserType);
                Assert.AreEqual(testUser.Roles, user.Roles);
                
                Assert.IsNotNull(user);
                Assert.AreEqual(testUser.FirstAndLastName, user.FirstAndLastName);
                Assert.AreEqual(testUser.MiddleName, user.MiddleName);
                Assert.AreEqual(testUser.NameSuffix, user.NameSuffix);
                Assert.AreEqual(testUser.Email, user.Email);
                Assert.AreEqual(testUser.Phone, user.Phone);
                Assert.IsNotNull(user.Address);
                Assert.AreEqual(testUser.Address.Street, user.Address.Street);
                Assert.AreEqual(testUser.Address.City, user.Address.City);
                Assert.AreEqual(testUser.Address.Region, user.Address.Region);
                Assert.AreEqual(testUser.Address.PostalCode, user.Address.PostalCode);
                Assert.AreEqual(testUser.Address.Street, user.Address.Street);
                Assert.IsNotNull(user.Address.Country);
                Assert.AreEqual(testUser.Address.Country.Name, user.Address.Country.Name);
                Assert.AreEqual(testUser.Address.Country.IsoCode, user.Address.Country.IsoCode);
            }

            var removeUserTask = new RemoveUser(DbContext);
            var removeUserResult = removeUserTask.DoTask(testUser);

            Assert.IsTrue(removeUserResult.Success);
            Assert.IsNull(removeUserResult.Exception);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new LoginUser(EmptyDbContext, new GetInstallation(EmptyDbContext), 
                new SeedSystemData(new SeedInstallation(EmptyDbContext), 
                                   new SeedCountries(EmptyDbContext), 
                                   new SeedPerformingRightsOrganizations(EmptyDbContext, new SeedCountries(EmptyDbContext)), 
                                   new SeedServices(EmptyDbContext), 
                                   new SeedPlatforms(EmptyDbContext, new ListServices(EmptyDbContext), new AddPlatform(EmptyDbContext)),
                                   new SeedGenres(EmptyDbContext),
                                   new SeedRecordingRoles(EmptyDbContext),
                                   new SeedMerchandiseCategories(EmptyDbContext)));
            
            var result = task.DoTask(null);
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
