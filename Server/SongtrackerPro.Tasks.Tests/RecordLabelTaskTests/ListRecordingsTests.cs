﻿using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.ArtistTasks;
using SongtrackerPro.Tasks.PublishingTasks;
using SongtrackerPro.Tasks.RecordLabelTasks;

namespace SongtrackerPro.Tasks.Tests.RecordLabelTaskTests
{
    [TestClass]
    public class ListRecordingsTests : TestsBase
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

            var task = new ListRecordings(DbContext);
            var result = task.DoTask(testRecordLabel);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);

            var recordings = result.Data;
            Assert.IsNotNull(recordings);
            Assert.IsTrue(recordings.Count >= 2);

            var recording1 = recordings.SingleOrDefault(r => r.Id == recording1Id);
            Assert.IsNotNull(recording1);
            Assert.AreEqual(testRecording1.Title, recording1.Title);
            Assert.AreEqual(testRecording1.IsCover, recording1.IsCover);
            Assert.AreEqual(testRecording1.IsLive, recording1.IsLive);
            Assert.AreEqual(testRecording1.IsRemix, recording1.IsRemix);
            Assert.AreEqual(testRecording1.Isrc, recording1.Isrc);
            Assert.AreEqual(testRecording1.SecondsLong, recording1.SecondsLong);
            Assert.IsNotNull(recording1.Artist);
            Assert.AreEqual(testArtist.Name, recording1.Artist.Name);
            Assert.IsNotNull(recording1.Genre);

            var recording2 = recordings.SingleOrDefault(r => r.Id == recording2Id);
            Assert.IsNotNull(recording2);
            Assert.AreEqual(testRecording2.Title, recording2.Title);
            Assert.AreEqual(testRecording2.IsCover, recording2.IsCover);
            Assert.AreEqual(testRecording2.IsLive, recording2.IsLive);
            Assert.AreEqual(testRecording2.IsRemix, recording2.IsRemix);
            Assert.AreEqual(testRecording2.Isrc, recording2.Isrc);
            Assert.AreEqual(testRecording2.SecondsLong, recording2.SecondsLong);
            Assert.IsNotNull(recording2.Artist);
            Assert.AreEqual(testArtist.Name, recording2.Artist.Name);
            Assert.IsNotNull(recording2.Genre);

            var removeRecordingTask = new RemoveRecording(DbContext);
            var removeRecordingResult = removeRecordingTask.DoTask(recording1);

            Assert.IsTrue(removeRecordingResult.Success);
            Assert.IsNull(removeRecordingResult.Exception);

            removeRecordingTask = new RemoveRecording(DbContext);
            removeRecordingResult = removeRecordingTask.DoTask(recording2);

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
            var removeCompositionResult = removeCompositionTask.DoTask(testComposition1);

            Assert.IsTrue(removeCompositionResult.Success);
            Assert.IsNull(removeCompositionResult.Exception);

            removeCompositionTask = new RemoveComposition(DbContext);
            removeCompositionResult = removeCompositionTask.DoTask(testComposition2);

            Assert.IsTrue(removeCompositionResult.Success);
            Assert.IsNull(removeCompositionResult.Exception);

            var removePublisherTask = new RemovePublisher(DbContext);
            var removePublisherResult = removePublisherTask.DoTask(testPublisher);

            Assert.IsTrue(removePublisherResult.Success);
            Assert.IsNull(removePublisherResult.Exception);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new ListRecordings(EmptyDbContext);
            var result = task.DoTask(new RecordLabel());
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
