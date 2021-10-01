using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.ArtistTasks;
using SongtrackerPro.Tasks.PublishingTasks;
using SongtrackerPro.Tasks.RecordLabelTasks;

namespace SongtrackerPro.Tasks.Tests.RecordLabelTaskTests
{
    [TestClass]
    public class AddRecordingTests : TestsBase
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

            var testComposition = TestsModel.Composition(testPublisher);
            var addCompositionTask = new AddComposition(DbContext);
            var addCompositionResult = addCompositionTask.DoTask(testComposition);
            
            Assert.IsTrue(addCompositionResult.Success);
            Assert.IsNull(addCompositionResult.Exception);
            Assert.IsNotNull(addCompositionResult.Data);

            var compositionId = addCompositionResult.Data;
            Assert.IsNotNull(compositionId);
            Assert.IsTrue(compositionId > 0);

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

            var testRecording = TestsModel.Recording(testComposition, testArtist, testRecordLabel);
            var task = new AddRecording(DbContext);
            var result = task.DoTask(testRecording);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Data);

            var recordingId = result.Data;
            Assert.IsNotNull(recordingId);
            Assert.IsTrue(recordingId > 0);

            var getRecordingTask = new GetRecording(DbContext);
            var recording = getRecordingTask.DoTask(recordingId.Value)?.Data;
            var formattingService = new FormattingService();

            Assert.IsNotNull(recording);
            Assert.AreEqual(testRecording.Title, recording.Title);
            Assert.AreEqual(testRecording.IsCover, recording.IsCover);
            Assert.AreEqual(testRecording.IsLive, recording.IsLive);
            Assert.AreEqual(testRecording.IsRemix, recording.IsRemix);
            Assert.AreEqual(testRecording.Isrc, recording.Isrc);
            Assert.AreEqual(testRecording.SecondsLong, recording.SecondsLong);
            Assert.IsNotNull(recording.Composition);
            Assert.AreEqual(testComposition.Title, recording.Composition.Title);
            Assert.AreEqual(testComposition.CatalogNumber, recording.Composition.CatalogNumber);
            Assert.AreEqual(testComposition.CopyrightedOn, recording.Composition.CopyrightedOn);
            Assert.AreEqual(testComposition.Iswc, recording.Composition.Iswc);
            Assert.IsNotNull(recording.Composition.Publisher);
            Assert.AreEqual(testPublisher.Name, recording.Composition.Publisher.Name);
            Assert.AreEqual(formattingService.FormatTaxId(testPublisher.TaxId), recording.Composition.Publisher.TaxId);
            Assert.AreEqual(testPublisher.Email, recording.Composition.Publisher.Email);
            Assert.AreEqual(formattingService.FormatPhoneNumber(testPublisher.Phone), recording.Composition.Publisher.Phone);
            Assert.IsNotNull(recording.Composition.Publisher.Address);
            Assert.AreEqual(testPublisher.Address.Street, recording.Composition.Publisher.Address.Street);
            Assert.AreEqual(testPublisher.Address.City, recording.Composition.Publisher.Address.City);
            Assert.AreEqual(testPublisher.Address.Region, recording.Composition.Publisher.Address.Region);
            Assert.AreEqual(testPublisher.Address.PostalCode, recording.Composition.Publisher.Address.PostalCode);
            Assert.IsNotNull(recording.Composition.Publisher.Address.Country);
            Assert.AreEqual(testPublisher.Address.Country.Name, recording.Composition.Publisher.Address.Country.Name);
            Assert.AreEqual(testPublisher.Address.Country.IsoCode, recording.Composition.Publisher.Address.Country.IsoCode);
            Assert.IsNotNull(recording.Composition.Publisher.PerformingRightsOrganization);
            Assert.AreEqual(testPublisher.PerformingRightsOrganization.Name, recording.Composition.Publisher.PerformingRightsOrganization.Name);
            Assert.IsNotNull(recording.Artist);
            Assert.AreEqual(testArtist.Name, recording.Artist.Name);
            Assert.AreEqual(formattingService.FormatTaxId(testArtist.TaxId), recording.Artist.TaxId);
            Assert.AreEqual(testArtist.Email, recording.Artist.Email);
            Assert.IsNotNull(recording.Artist.Address);
            Assert.AreEqual(testArtist.Address.Street, recording.Artist.Address.Street);
            Assert.AreEqual(testArtist.Address.City, recording.Artist.Address.City);
            Assert.AreEqual(testArtist.Address.Region, recording.Artist.Address.Region);
            Assert.AreEqual(testArtist.Address.PostalCode, recording.Artist.Address.PostalCode);
            Assert.IsNotNull(recording.Artist.Address.Country);
            Assert.AreEqual(testArtist.Address.Country.Name, recording.Artist.Address.Country.Name);
            Assert.AreEqual(testArtist.Address.Country.IsoCode, recording.Artist.Address.Country.IsoCode);
            Assert.AreEqual(testArtist.HasServiceMark, recording.Artist.HasServiceMark);
            Assert.AreEqual(testArtist.WebsiteUrl, recording.Artist.WebsiteUrl);
            Assert.AreEqual(testArtist.PressKitUrl, recording.Artist.PressKitUrl);
            if (testArtist.RecordLabel != null)
                Assert.AreEqual(testArtist.RecordLabel.Name, recording.Artist.RecordLabel.Name);
            Assert.IsNotNull(recording.RecordLabel);
            Assert.AreEqual(testRecordLabel.Name, recording.RecordLabel.Name);
            Assert.AreEqual(formattingService.FormatTaxId(testRecordLabel.TaxId), recording.RecordLabel.TaxId);
            Assert.AreEqual(testRecordLabel.Email, recording.RecordLabel.Email);
            Assert.AreEqual(formattingService.FormatPhoneNumber(testRecordLabel.Phone), recording.RecordLabel.Phone);
            Assert.IsNotNull(testRecordLabel.Address);
            Assert.AreEqual(testRecordLabel.Address.Street, recording.RecordLabel.Address.Street);
            Assert.AreEqual(testRecordLabel.Address.City, recording.RecordLabel.Address.City);
            Assert.AreEqual(testRecordLabel.Address.Region, recording.RecordLabel.Address.Region);
            Assert.AreEqual(testRecordLabel.Address.PostalCode, recording.RecordLabel.Address.PostalCode);
            Assert.IsNotNull(testRecordLabel.Address.Country);
            Assert.AreEqual(testRecordLabel.Address.Country.Name, recording.RecordLabel.Address.Country.Name);
            Assert.AreEqual(testRecordLabel.Address.Country.IsoCode, recording.RecordLabel.Address.Country.IsoCode);
            Assert.IsNotNull(recording.Genre);

            var removeRecordingTask = new RemoveRecording(DbContext);
            var removeResult = removeRecordingTask.DoTask(recording);

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

            var removeCompositionTask = new RemoveComposition(DbContext);
            var removeCompositionResult = removeCompositionTask.DoTask(testComposition);

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
            var task = new AddRecording(EmptyDbContext);
            var result = task.DoTask(new Recording());
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
