using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.ArtistTasks;
using SongtrackerPro.Tasks.RecordLabelTasks;

namespace SongtrackerPro.Tasks.Tests.RecordLabelTaskTests
{
    [TestClass]
    public class ListReleasesTests : TestsBase
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

            var testRelease1 = TestsModel.Release(testArtist, testRecordLabel);
            var addReleaseTask = new AddRelease(DbContext);
            var addReleaseResult = addReleaseTask.DoTask(testRelease1);

            Assert.IsTrue(addReleaseResult.Success);
            Assert.IsNull(addReleaseResult.Exception);
            Assert.IsNotNull(addReleaseResult.Data);

            var release1Id = addReleaseResult.Data;
            Assert.IsNotNull(release1Id);
            Assert.IsTrue(release1Id > 0);

            var testRelease2 = TestsModel.Release(null, testRecordLabel);
            addReleaseTask = new AddRelease(DbContext);
            addReleaseResult = addReleaseTask.DoTask(testRelease2);

            Assert.IsTrue(addReleaseResult.Success);
            Assert.IsNull(addReleaseResult.Exception);
            Assert.IsNotNull(addReleaseResult.Data);

            var release2Id = addReleaseResult.Data;
            Assert.IsNotNull(release2Id);
            Assert.IsTrue(release2Id > 0);

            var task = new ListReleases(DbContext);
            var result = task.DoTask(testRecordLabel);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);

            var releases = result.Data;
            Assert.IsNotNull(releases);
            Assert.IsTrue(releases.Count >= 2);

            var release1 = releases.SingleOrDefault(r => r.Id == release1Id);
            Assert.IsNotNull(release1);
            Assert.AreEqual(testRelease1.Title, release1.Title);
            Assert.AreEqual(testRelease1.CatalogNumber, release1.CatalogNumber);
            Assert.AreEqual(testRelease1.Type, release1.Type);
            Assert.IsNotNull(release1.Artist);
            Assert.AreEqual(testArtist.Name, release1.Artist.Name);
            Assert.IsNotNull(release1.Genre);

            var release2 = releases.SingleOrDefault(r => r.Id == release2Id);
            Assert.IsNotNull(release2);
            Assert.AreEqual(testRelease2.Title, release2.Title);
            Assert.AreEqual(testRelease2.CatalogNumber, release2.CatalogNumber);
            Assert.AreEqual(testRelease2.Type, release2.Type);
            Assert.IsNull(release2.Artist);
            Assert.IsNotNull(release2.Genre);

            var removeReleaseTask = new RemoveRelease(DbContext);
            var removeReleaseResult = removeReleaseTask.DoTask(release1);

            Assert.IsTrue(removeReleaseResult.Success);
            Assert.IsNull(removeReleaseResult.Exception);

            removeReleaseTask = new RemoveRelease(DbContext);
            removeReleaseResult = removeReleaseTask.DoTask(release2);

            Assert.IsTrue(removeReleaseResult.Success);
            Assert.IsNull(removeReleaseResult.Exception);

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
            var task = new ListReleases(EmptyDbContext);
            var result = task.DoTask(new RecordLabel());
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
