using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.ArtistTasks;
using SongtrackerPro.Tasks.PublishingTasks;
using SongtrackerPro.Tasks.RecordLabelTasks;

namespace SongtrackerPro.Tasks.Tests.RecordLabelTaskTests
{
    [TestClass]
    public class ListReleaseTracksTests : TestsBase
    {
        [TestMethod]
        public void TaskSuccessTest()
        {
            var addPublisherTask = new AddPublisher(DbContext, new FormattingService());
            var testPublisher = TestsModel.Publisher;
            var addPublisherResult = addPublisherTask.DoTask(testPublisher);

            Assert.IsTrue(addPublisherResult.Success);
            Assert.IsNull(addPublisherResult.Exception);
            Assert.IsNotNull(addPublisherResult.Data);

            var publisherId = addPublisherResult.Data;
            Assert.IsNotNull(publisherId);
            Assert.IsTrue(publisherId > 0);

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
            Assert.IsNotNull(addReleaseResult.Data);

            var releaseId = addReleaseResult.Data;
            Assert.IsNotNull(releaseId);
            Assert.IsTrue(releaseId > 0);

            var testComposition1 = TestsModel.Composition(testPublisher);
            var addCompositionTask = new AddComposition(DbContext);
            var addCompositionResult = addCompositionTask.DoTask(testComposition1);
            
            Assert.IsTrue(addCompositionResult.Success);
            Assert.IsNull(addCompositionResult.Exception);
            Assert.IsNotNull(addCompositionResult.Data);

            var composition1Id = addCompositionResult.Data;
            Assert.IsNotNull(composition1Id);
            Assert.IsTrue(composition1Id > 0);

            var testComposition2 = TestsModel.Composition(testPublisher);
            addCompositionTask = new AddComposition(DbContext);
            addCompositionResult = addCompositionTask.DoTask(testComposition2);
            
            Assert.IsTrue(addCompositionResult.Success);
            Assert.IsNull(addCompositionResult.Exception);
            Assert.IsNotNull(addCompositionResult.Data);

            var composition2Id = addCompositionResult.Data;
            Assert.IsNotNull(composition2Id);
            Assert.IsTrue(composition2Id > 0);

            var testRecording1 = TestsModel.Recording(testComposition1, testArtist, testRecordLabel);
            var addRecordingTask = new AddRecording(DbContext);
            var addRecordingResult = addRecordingTask.DoTask(testRecording1);

            Assert.IsTrue(addRecordingResult.Success);
            Assert.IsNull(addRecordingResult.Exception);
            Assert.IsNotNull(addRecordingResult.Data);

            var recording1Id = addRecordingResult.Data;
            Assert.IsNotNull(recording1Id);
            Assert.IsTrue(recording1Id > 0);

            var testRecording2 = TestsModel.Recording(testComposition1, testArtist, testRecordLabel);
            addRecordingTask = new AddRecording(DbContext);
            addRecordingResult = addRecordingTask.DoTask(testRecording2);

            Assert.IsTrue(addRecordingResult.Success);
            Assert.IsNull(addRecordingResult.Exception);
            Assert.IsNotNull(addRecordingResult.Data);

            var recording2Id = addRecordingResult.Data;
            Assert.IsNotNull(recording2Id);
            Assert.IsTrue(recording2Id > 0);

            var testReleaseTrack1 = TestsModel.ReleaseTrack(testRelease, testRecording1, 1);
            var addReleaseTrackTask = new AddReleaseTrack(DbContext);
            var addReleaseTrackResult = addReleaseTrackTask.DoTask(testReleaseTrack1);

            Assert.IsTrue(addReleaseTrackResult.Success);
            Assert.IsNull(addReleaseTrackResult.Exception);
            Assert.IsNotNull(addReleaseTrackResult.Data);

            var releaseTrack1Id = addReleaseTrackResult.Data;
            Assert.IsNotNull(releaseTrack1Id);
            Assert.IsTrue(releaseTrack1Id > 0);

            var testReleaseTrack2 = TestsModel.ReleaseTrack(testRelease, testRecording2, 2);
            addReleaseTrackTask = new AddReleaseTrack(DbContext);
            addReleaseTrackResult = addReleaseTrackTask.DoTask(testReleaseTrack2);

            Assert.IsTrue(addReleaseTrackResult.Success);
            Assert.IsNull(addReleaseTrackResult.Exception);
            Assert.IsNotNull(addReleaseTrackResult.Data);

            var releaseTrack2Id = addReleaseTrackResult.Data;
            Assert.IsNotNull(releaseTrack2Id);
            Assert.IsTrue(releaseTrack2Id > 0);

            var task = new ListReleaseTracks(DbContext);
            var result = task.DoTask(testRelease);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);

            var releaseTracks = result.Data;
            Assert.IsNotNull(releaseTracks);
            Assert.IsTrue(releaseTracks.Count >= 2);

            Assert.AreEqual(testReleaseTrack1.TrackNumber, releaseTracks[0].TrackNumber);
            Assert.IsNotNull(releaseTracks[0].Recording);
            Assert.AreEqual(testRecording1.Title, releaseTracks[0].Recording.Title);
            Assert.AreEqual(testRecording1.Isrc, releaseTracks[0].Recording.Isrc);

            Assert.AreEqual(testReleaseTrack2.TrackNumber, releaseTracks[1].TrackNumber);
            Assert.IsNotNull(releaseTracks[1].Recording);
            Assert.AreEqual(testRecording2.Title, releaseTracks[1].Recording.Title);
            Assert.AreEqual(testRecording2.Isrc, releaseTracks[1].Recording.Isrc);

            var removeReleaseTrackTask = new RemoveReleaseTrack(DbContext);
            var removeReleaseTrackResult = removeReleaseTrackTask.DoTask(testReleaseTrack1);

            Assert.IsTrue(removeReleaseTrackResult.Success);
            Assert.IsNull(removeReleaseTrackResult.Exception);

            removeReleaseTrackTask = new RemoveReleaseTrack(DbContext);
            removeReleaseTrackResult = removeReleaseTrackTask.DoTask(testReleaseTrack2);

            Assert.IsTrue(removeReleaseTrackResult.Success);
            Assert.IsNull(removeReleaseTrackResult.Exception);

            var removeReleaseTask = new RemoveRelease(DbContext);
            var removeReleaseResult = removeReleaseTask.DoTask(testRelease);

            Assert.IsTrue(removeReleaseResult.Success);
            Assert.IsNull(removeReleaseResult.Exception);

            var removeRecordingTask = new RemoveRecording(DbContext);
            var removeRecordingResult = removeRecordingTask.DoTask(testRecording1);

            Assert.IsTrue(removeRecordingResult.Success);
            Assert.IsNull(removeRecordingResult.Exception);

            removeRecordingTask = new RemoveRecording(DbContext);
            removeRecordingResult = removeRecordingTask.DoTask(testRecording2);

            Assert.IsTrue(removeRecordingResult.Success);
            Assert.IsNull(removeRecordingResult.Exception);

            var removeRecordLabelTask = new RemoveRecordLabel(DbContext);
            var removeRecordLabelResult = removeRecordLabelTask.DoTask(testRecordLabel);

            Assert.IsTrue(removeRecordLabelResult.Success);
            Assert.IsNull(removeRecordLabelResult.Exception);

            var removeArtistTask = new RemoveArtist(DbContext);
            var removeArtistResult = removeArtistTask.DoTask(testArtist);

            Assert.IsTrue(removeArtistResult.Success);
            Assert.IsNull(removeArtistResult.Exception);

            var removeCompositionTask = new RemoveComposition(DbContext);
            var removeCompositionResult1 = removeCompositionTask.DoTask(testComposition1);
            var removeCompositionResult2 = removeCompositionTask.DoTask(testComposition2);

            Assert.IsTrue(removeCompositionResult1.Success);
            Assert.IsNull(removeCompositionResult1.Exception);

            Assert.IsTrue(removeCompositionResult2.Success);
            Assert.IsNull(removeCompositionResult2.Exception);

            var removePublisherTask = new RemovePublisher(DbContext);
            var removePublisherResult = removePublisherTask.DoTask(testPublisher);

            Assert.IsTrue(removePublisherResult.Success);
            Assert.IsNull(removePublisherResult.Exception);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new ListReleaseTracks(EmptyDbContext);
            var result = task.DoTask(new Release());
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
