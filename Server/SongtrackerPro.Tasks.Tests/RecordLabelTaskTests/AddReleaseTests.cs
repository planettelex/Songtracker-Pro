using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.ArtistTasks;
using SongtrackerPro.Tasks.RecordLabelTasks;

namespace SongtrackerPro.Tasks.Tests.RecordLabelTaskTests
{
    [TestClass]
    public class AddReleaseTests : TestsBase
    {
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
            var task = new AddRelease(DbContext);
            var result = task.DoTask(testRelease);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Data);

            var releaseId = result.Data;
            Assert.IsNotNull(releaseId);
            Assert.IsTrue(releaseId > 0);

            var getReleaseTask = new GetRelease(DbContext);
            var release = getReleaseTask.DoTask(releaseId.Value)?.Data;

            Assert.IsNotNull(release);
            Assert.AreEqual(testRelease.Title, release.Title);
            Assert.AreEqual(testRelease.CatalogNumber, release.CatalogNumber);
            Assert.AreEqual(testRelease.Type, release.Type);
            Assert.IsNotNull(release.Artist);
            Assert.AreEqual(testArtist.Name, release.Artist.Name);
            Assert.AreEqual(testArtist.Email, release.Artist.Email);
            Assert.AreEqual(testArtist.TaxId, release.Artist.TaxId);
            Assert.IsNotNull(release.RecordLabel);
            Assert.AreEqual(testRecordLabel.Name, release.RecordLabel.Name);
            Assert.AreEqual(testRecordLabel.Email, release.RecordLabel.Email);
            Assert.AreEqual(testRecordLabel.TaxId, release.RecordLabel.TaxId);

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
            var task = new AddRelease(EmptyDbContext);
            var result = task.DoTask(new Release());
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
