using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.ArtistTasks;
using SongtrackerPro.Tasks.PlatformTasks;

namespace SongtrackerPro.Tasks.Tests.ArtistTaskTests
{
    [TestClass]
    public class AddArtistLinkTests : TestsBase
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

            var paymentService = new ListServices(DbContext).DoTask(null).Data.SingleOrDefault(s => s.Name.ToLower() == "payment");
            Assert.IsNotNull(paymentService);

            var allPlatforms = new ListPlatforms(DbContext).DoTask(null).Data.ToList();
            var linkPlatforms = new List<Platform>();
            foreach (var platform in allPlatforms)
                linkPlatforms.AddRange(from service in platform.Services where service.Id != paymentService.Id select platform);

            var linkPlatform = linkPlatforms[new Random().Next(0, linkPlatforms.Count)];

            var artistLink = new ArtistLink
            {
                Platform = linkPlatform,
                Artist = testArtist,
                Url = "http://www." + DateTime.Now.Ticks + ".com"
            };

            var task = new AddArtistLink(DbContext);
            var result = task.DoTask(artistLink);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Data);

            var getArtistLinkTask = new GetArtistLink(DbContext);
            var getArtistLinkResult = getArtistLinkTask.DoTask(artistLink.Id);

            Assert.IsTrue(getArtistLinkResult.Success);
            Assert.IsNull(getArtistLinkResult.Exception);
            Assert.IsNotNull(getArtistLinkResult.Data);

            Assert.AreEqual(artistLink.PlatformId, getArtistLinkResult.Data.PlatformId);
            Assert.AreEqual(artistLink.ArtistId, getArtistLinkResult.Data.ArtistId);
            Assert.AreEqual(artistLink.Url, getArtistLinkResult.Data.Url);

            var removeArtistTask = new RemoveArtist(DbContext);
            var removeArtistResult = removeArtistTask.DoTask(testArtist);

            Assert.IsTrue(removeArtistResult.Success);
            Assert.IsNull(removeArtistResult.Exception);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new AddArtistLink(EmptyDbContext);
            var result = task.DoTask(new ArtistLink());
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
