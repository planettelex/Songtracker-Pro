using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.ArtistTasks;
using SongtrackerPro.Tasks.PublishingTasks;
using SongtrackerPro.Tasks.RecordLabelTasks;

namespace SongtrackerPro.Tasks.Tests.RecordLabelTaskTests
{
    [TestClass]
    public class UpdateReleaseTrackTests : TestsBase
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

            var testComposition = TestsModel.Composition(testPublisher);
            var addCompositionTask = new AddComposition(DbContext);
            var addCompositionResult = addCompositionTask.DoTask(testComposition);
            
            Assert.IsTrue(addCompositionResult.Success);
            Assert.IsNull(addCompositionResult.Exception);
            Assert.IsNotNull(addCompositionResult.Data);

            var compositionId = addCompositionResult.Data;
            Assert.IsNotNull(compositionId);
            Assert.IsTrue(compositionId > 0);

            var testRecording = TestsModel.Recording(testComposition, testArtist, testRecordLabel);
            var addRecordingTask = new AddRecording(DbContext);
            var addRecordingResult = addRecordingTask.DoTask(testRecording);

            Assert.IsTrue(addRecordingResult.Success);
            Assert.IsNull(addRecordingResult.Exception);
            Assert.IsNotNull(addRecordingResult.Data);

            var recordingId = addRecordingResult.Data;
            Assert.IsNotNull(recordingId);
            Assert.IsTrue(recordingId > 0);

            var testReleaseTrack = TestsModel.ReleaseTrack(testRelease, testRecording, 1);
            var addReleaseTrackTask = new AddReleaseTrack(DbContext);
            var addReleaseTrackResult = addReleaseTrackTask.DoTask(testReleaseTrack);

            Assert.IsTrue(addReleaseTrackResult.Success);
            Assert.IsNull(addReleaseTrackResult.Exception);
            Assert.IsNotNull(addReleaseTrackResult.Data);

            var releaseTrackId = addReleaseTrackResult.Data;
            Assert.IsNotNull(releaseTrackId);
            Assert.IsTrue(releaseTrackId > 0);

            const int newTrackNumber = 5;
            testReleaseTrack.TrackNumber = newTrackNumber;

            var task = new UpdateReleaseTrack(DbContext);
            var result = task.DoTask(testReleaseTrack);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);

            var listReleaseTracksTask = new ListReleaseTracks(DbContext);
            var listReleaseTracksResult = listReleaseTracksTask.DoTask(testRelease);
            
            Assert.IsTrue(listReleaseTracksResult.Success);
            Assert.IsNull(listReleaseTracksResult.Exception);
            Assert.IsNotNull(listReleaseTracksResult.Data);
            Assert.AreEqual(newTrackNumber,listReleaseTracksResult.Data[0].TrackNumber);

            var removeReleaseTask = new RemoveRelease(DbContext);
            var removeReleaseResult = removeReleaseTask.DoTask(testRelease);

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

            var removePublisherTask = new RemovePublisher(DbContext);
            var removePublisherResult = removePublisherTask.DoTask(testPublisher);

            Assert.IsTrue(removePublisherResult.Success);
            Assert.IsNull(removePublisherResult.Exception);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new UpdateReleaseTrack(EmptyDbContext);
            var result = task.DoTask(null);
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
