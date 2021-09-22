using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Enums;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.ArtistTasks;
using SongtrackerPro.Tasks.RecordLabelTasks;

namespace SongtrackerPro.Tasks.Tests.RecordLabelTaskTests
{
    [TestClass]
    public class UpdateReleaseTests : TestsBase
    {
        private void UpdateReleaseModel(Release release)
        {
            var stamp = DateTime.Now.Ticks;
            release.Title = "Update " + stamp;
            release.CatalogNumber = "#" + stamp;
            release.Type = ReleaseType.Ep;
        }

        [TestMethod]
        public void TaskSuccessTest()
        {
            var testArtist = TestsModel.Artist;
            var addArtistTask = new AddArtist(DbContext, new FormattingService());
            var addArtistResult = addArtistTask.DoTask(testArtist);

            Assert.IsTrue(addArtistResult.Success);
            Assert.IsNull(addArtistResult.Exception);
            Assert.IsNotNull(addArtistResult.Data);

            var artistId = addArtistResult.Data;
            Assert.IsNotNull(artistId);
            Assert.IsTrue(artistId > 0);

            var testRecordLabel = TestsModel.RecordLabel;
            var addRecordLabelTask = new AddRecordLabel(DbContext, new FormattingService());
            var addRecordLabelResult = addRecordLabelTask.DoTask(testRecordLabel);

            Assert.IsTrue(addRecordLabelResult.Success);
            Assert.IsNull(addRecordLabelResult.Exception);
            Assert.IsNotNull(addRecordLabelResult.Data);

            var recordLabelId = addRecordLabelResult.Data;
            Assert.IsNotNull(recordLabelId);
            Assert.IsTrue(recordLabelId > 0);

            var testRelease = TestsModel.Release(testArtist, testRecordLabel);
            var addReleaseTask = new AddRelease(DbContext);
            var addReleaseResult = addReleaseTask.DoTask(testRelease);

            Assert.IsTrue(addReleaseResult.Success);
            Assert.IsNull(addReleaseResult.Exception);

            var releaseId = addReleaseResult.Data;
            Assert.IsNotNull(releaseId);
            Assert.IsTrue(releaseId > 0);

            var task = new UpdateRelease(DbContext);
            UpdateReleaseModel(testRelease);
            var result = task.DoTask(testRelease);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNull(result.Data);

            var getReleaseTask = new GetRelease(DbContext);
            var release = getReleaseTask.DoTask(releaseId.Value)?.Data;

            Assert.IsNotNull(release);
            Assert.AreEqual(testRelease.Title, release.Title);
            Assert.AreEqual(testRelease.CatalogNumber, release.CatalogNumber);
            Assert.AreEqual(testRelease.Type, release.Type);

            var removeReleaseTask = new RemoveRelease(DbContext);
            var removeResult = removeReleaseTask.DoTask(release);

            Assert.IsTrue(removeResult.Success);
            Assert.IsNull(removeResult.Exception);

            var removeRecordLabelTask = new RemoveRecordLabel(DbContext);
            var removeRecordLabelResult = removeRecordLabelTask.DoTask(testRecordLabel);

            Assert.IsTrue(removeRecordLabelResult.Success);
            Assert.IsNull(removeRecordLabelResult.Exception);

            var removeArtistTask = new RemoveArtist(DbContext);
            var removeArtistResult = removeArtistTask.DoTask(testArtist);

            Assert.IsTrue(removeArtistResult.Success);
            Assert.IsNull(removeArtistResult.Exception);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new UpdateRelease(EmptyDbContext);
            var result = task.DoTask(null);
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
