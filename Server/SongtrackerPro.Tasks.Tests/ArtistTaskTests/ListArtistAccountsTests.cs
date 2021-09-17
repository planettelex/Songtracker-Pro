using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Enums;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.ArtistTasks;
using SongtrackerPro.Tasks.InstallationTasks;
using SongtrackerPro.Tasks.PlatformTasks;

namespace SongtrackerPro.Tasks.Tests.ArtistTaskTests
{
    [TestClass]
    public class ListArtistAccountsTests : TestsBase
    {
        [TestMethod]
        public void TaskSuccessTest()
        {
            var addArtistTask = new AddArtist(DbContext, new FormattingService());
            var testArtist = TestsModel.Artist;
            var addArtistResult = addArtistTask.DoTask(testArtist);

            Assert.IsTrue(addArtistResult.Success);
            Assert.IsNull(addArtistResult.Exception);
            Assert.IsNotNull(addArtistResult.Data);

            var artistId = addArtistResult.Data;
            Assert.IsNotNull(artistId);
            Assert.IsTrue(artistId > 0);

            var paymentService = new ListServices(DbContext).DoTask(ServiceType.Platform).Data
                .SingleOrDefault(s => s.Name.ToLower() == "payment");
            Assert.IsNotNull(paymentService);

            var allPlatforms = new ListPlatforms(DbContext).DoTask(null).Data.ToList();
            var paymentPlatforms = new List<Platform>();
            foreach (var platform in allPlatforms)
                paymentPlatforms.AddRange(from service in platform.Services where service.Id == paymentService.Id select platform);

            foreach (var paymentPlatform in paymentPlatforms)
            {
                var artistAccount = new ArtistAccount
                {
                    IsPreferred = new Random().Next(0, 2) == 0,
                    Platform = paymentPlatform,
                    Artist = testArtist,
                    Username = "@artist-" + new Random().Next(100, 999)
                };
                var addArtistAccountTask = new AddArtistAccount(DbContext);
                var addArtistAccountResult = addArtistAccountTask.DoTask(artistAccount);

                Assert.IsTrue(addArtistAccountResult.Success);
                Assert.IsNull(addArtistAccountResult.Exception);
                Assert.IsNotNull(addArtistAccountResult.Data);
            }

            var task = new ListArtistAccounts(DbContext);
            var result = task.DoTask(testArtist);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Data);
            Assert.AreEqual(paymentPlatforms.Count, result.Data.Count);

            foreach (var userAccount in result.Data)
            {
                Assert.AreEqual(userAccount.ArtistId, testArtist.Id);
                Assert.IsNotNull(userAccount.Username);
            }
            
            var removeArtistTask = new RemoveArtist(DbContext);
            var removeArtistResult = removeArtistTask.DoTask(testArtist);

            Assert.IsTrue(removeArtistResult.Success);
            Assert.IsNull(removeArtistResult.Exception);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new AddArtistAccount(EmptyDbContext);
            var result = task.DoTask(new ArtistAccount());
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
