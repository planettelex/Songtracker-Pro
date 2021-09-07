using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.ArtistTasks;
using SongtrackerPro.Tasks.RecordLabelTasks;

namespace SongtrackerPro.Tasks.Tests.ArtistTaskTests
{
    [TestClass]
    public class UpdateArtistTests : TestsBase
    {
        public void UpdateArtistModel(Artist artist)
        {
            var stamp = DateTime.Now.Ticks;
            artist.Name = "Update " + stamp;
            artist.TaxId = stamp.ToString();
            artist.Email = $"test@update{stamp}.com";
            artist.Address = TestsModel.Address;
            artist.HasServiceMark = new Random().Next(0, 2) == 0;
            artist.WebsiteUrl = "http://website-update.com";
            artist.PressKitUrl = "http://epk-update.com";

            var listRecordLabelsTask = new ListRecordLabels(DbContext);
            var listRecordLabelResult = listRecordLabelsTask.DoTask(null);
            var recordLabels = listRecordLabelResult.Data;
            if (recordLabels != null && recordLabels.Any())
            {
                var newRecordLabelIndex = new Random().Next(0, recordLabels.Count);
                artist.RecordLabel = recordLabels[newRecordLabelIndex];
                artist.RecordLabelId = artist.RecordLabel.Id;
            }
        }

        [TestMethod]
        public void TaskSuccessTest()
        {
            var testArtist = TestsModel.Artist;
            var addArtistTask = new AddArtist(DbContext, new FormattingService());
            var addArtistResult = addArtistTask.DoTask(testArtist);

            Assert.IsTrue(addArtistResult.Success);
            Assert.IsNull(addArtistResult.Exception);

            var task = new UpdateArtist(DbContext, new FormattingService());
            var toUpdate = testArtist;
            UpdateArtistModel(toUpdate);
            var result = task.DoTask(toUpdate);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNull(result.Data);

            var getArtistTask = new GetArtist(DbContext);
            var artist = getArtistTask.DoTask(toUpdate.Id)?.Data;
            var formattingService = new FormattingService();

            Assert.IsNotNull(artist);
            Assert.AreEqual(toUpdate.Name, artist.Name);
            Assert.AreEqual(formattingService.FormatTaxId(toUpdate.TaxId), artist.TaxId);
            Assert.AreEqual(toUpdate.Email, artist.Email);
            Assert.AreEqual(toUpdate.Address.Street, artist.Address.Street);
            Assert.AreEqual(toUpdate.Address.City, artist.Address.City);
            Assert.AreEqual(toUpdate.Address.Region, artist.Address.Region);
            Assert.AreEqual(toUpdate.Address.PostalCode, artist.Address.PostalCode);
            Assert.AreEqual(toUpdate.Address.Country.Name, artist.Address.Country.Name);
            Assert.AreEqual(toUpdate.HasServiceMark, artist.HasServiceMark);
            Assert.AreEqual(toUpdate.WebsiteUrl, artist.WebsiteUrl);
            Assert.AreEqual(toUpdate.PressKitUrl, artist.PressKitUrl);
            if (testArtist.RecordLabel != null)
            {
                Assert.AreEqual(toUpdate.RecordLabel.Name, artist.RecordLabel.Name);
                Assert.AreEqual(formattingService.FormatTaxId(toUpdate.RecordLabel.TaxId), artist.RecordLabel.TaxId);
                Assert.AreEqual(toUpdate.RecordLabel.Email, artist.RecordLabel.Email);
                Assert.AreEqual(formattingService.FormatPhoneNumber(toUpdate.RecordLabel.Phone), artist.RecordLabel.Phone);
                Assert.IsNotNull(toUpdate.RecordLabel.Address);
                Assert.AreEqual(toUpdate.RecordLabel.Address.Street, artist.RecordLabel.Address.Street);
                Assert.AreEqual(toUpdate.RecordLabel.Address.City, artist.RecordLabel.Address.City);
                Assert.AreEqual(toUpdate.RecordLabel.Address.Region, artist.RecordLabel.Address.Region);
                Assert.AreEqual(toUpdate.RecordLabel.Address.PostalCode, artist.RecordLabel.Address.PostalCode);
                Assert.IsNotNull(toUpdate.RecordLabel.Address.Country);
                Assert.AreEqual(toUpdate.RecordLabel.Address.Country.Name, artist.RecordLabel.Address.Country.Name);
                Assert.AreEqual(toUpdate.RecordLabel.Address.Country.IsoCode, artist.RecordLabel.Address.Country.IsoCode);
            }

            var removeArtistTask = new RemoveArtist(DbContext);
            var removeArtistResult = removeArtistTask.DoTask(artist);

            Assert.IsTrue(removeArtistResult.Success);
            Assert.IsNull(removeArtistResult.Exception);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new UpdateArtist(EmptyDbContext, new FormattingService());
            var result = task.DoTask(null);
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
