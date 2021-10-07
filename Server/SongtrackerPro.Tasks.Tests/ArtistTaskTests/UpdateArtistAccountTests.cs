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
    public class UpdateArtistAccountTests : TestsBase
    {
        [TestMethod]
        public void TaskSuccessTest()
        {
            var testArtist = TestsModel.Artist;
            var addArtistTask = new AddArtist(DbContext, new FormattingService());
            var addArtistResult = addArtistTask.DoTask(testArtist);

            Assert.IsTrue(addArtistResult.Success);
            Assert.IsNull(addArtistResult.Exception);

            var paymentService = new ListServices(DbContext).DoTask(ServiceType.Platform).Data
                .SingleOrDefault(s => s.Name.ToLower() == "payment");
            Assert.IsNotNull(paymentService);

            var allPlatforms = new ListPlatforms(DbContext).DoTask(null).Data.ToList();
            var paymentPlatforms = new List<Platform>();
            foreach (var platform in allPlatforms)
                paymentPlatforms.AddRange(from service in platform.Services where service.Id == paymentService.Id select platform);

            var paymentPlatform = paymentPlatforms[new Random().Next(0, paymentPlatforms.Count)];

            var artistAccount = new ArtistAccount
            {
                IsPreferred = true,
                Platform = paymentPlatform,
                Artist = testArtist,
                Username = "@artist-" + DateTime.Now.Ticks
            };

            var addArtistAccountTask = new AddArtistAccount(DbContext);
            var addArtistAccountResult = addArtistAccountTask.DoTask(artistAccount);

            Assert.IsTrue(addArtistAccountResult.Success);
            Assert.IsNull(addArtistAccountResult.Exception);
            Assert.IsNotNull(addArtistAccountResult.Data);

            var getArtistAccountTask = new GetArtistAccount(DbContext);
            var getArtistAccountResult = getArtistAccountTask.DoTask(artistAccount.Id);

            Assert.IsTrue(getArtistAccountResult.Success);
            Assert.IsNull(getArtistAccountResult.Exception);
            Assert.IsNotNull(getArtistAccountResult.Data);

            Assert.AreEqual(artistAccount.PlatformId, getArtistAccountResult.Data.PlatformId);
            Assert.AreEqual(artistAccount.ArtistId, getArtistAccountResult.Data.ArtistId);
            Assert.AreEqual(artistAccount.IsPreferred, getArtistAccountResult.Data.IsPreferred);
            Assert.AreEqual(artistAccount.Username, getArtistAccountResult.Data.Username);

            artistAccount.Username = "@update-" + DateTime.Now.Ticks;
            artistAccount.IsPreferred = false;

            var task = new UpdateArtistAccount(DbContext);
            var result = task.DoTask(artistAccount);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);

            getArtistAccountTask = new GetArtistAccount(DbContext);
            getArtistAccountResult = getArtistAccountTask.DoTask(artistAccount.Id);
            Assert.AreEqual(artistAccount.PlatformId, getArtistAccountResult.Data.PlatformId);
            Assert.AreEqual(artistAccount.ArtistId, getArtistAccountResult.Data.ArtistId);
            Assert.AreEqual(artistAccount.IsPreferred, getArtistAccountResult.Data.IsPreferred);
            Assert.AreEqual(artistAccount.Username, getArtistAccountResult.Data.Username);

            var removeArtistTask = new RemoveArtist(DbContext);
            var removeArtistResult = removeArtistTask.DoTask(testArtist);

            Assert.IsTrue(removeArtistResult.Success);
            Assert.IsNull(removeArtistResult.Exception);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new UpdateArtistAccount(EmptyDbContext);
            var result = task.DoTask(null);
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
