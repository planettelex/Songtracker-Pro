using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.ArtistTasks;

namespace SongtrackerPro.Tasks.Tests.ArtistTaskTests
{
    [TestClass]
    public class GetArtistTests : TestsBase
    {
        [TestMethod]
        public void TaskSuccessTest()
        {
            var addArtistTask = new AddArtist(DbContext, new FormattingService());
            var testArtist = TestsModel.Artist;
            var testArtistId = addArtistTask.DoTask(testArtist);
            Assert.IsTrue(testArtistId.Data.HasValue);

            var task = new GetArtist(DbContext);
            var result = task.DoTask(testArtistId.Data.Value);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);

            var artist = result.Data;
            Assert.IsNotNull(artist);
            Assert.AreEqual(testArtist.Name, artist.Name);
            Assert.AreEqual(testArtist.TaxId, artist.TaxId);
            Assert.AreEqual(testArtist.Email, artist.Email);
            Assert.IsNotNull(artist.Address);
            Assert.AreEqual(testArtist.Address.Street, artist.Address.Street);
            Assert.AreEqual(testArtist.Address.City, artist.Address.City);
            Assert.AreEqual(testArtist.Address.Region, artist.Address.Region);
            Assert.AreEqual(testArtist.Address.PostalCode, artist.Address.PostalCode);
            Assert.IsNotNull(artist.Address.Country);
            Assert.AreEqual(testArtist.Address.Country.Name, artist.Address.Country.Name);
            Assert.AreEqual(testArtist.Address.Country.IsoCode, artist.Address.Country.IsoCode);
            Assert.AreEqual(testArtist.HasServiceMark, artist.HasServiceMark);
            Assert.AreEqual(testArtist.WebsiteUrl, artist.WebsiteUrl);
            Assert.AreEqual(testArtist.PressKitUrl, artist.PressKitUrl);
            if (testArtist.RecordLabel != null)
            {
                Assert.AreEqual(testArtist.RecordLabel.Name, artist.RecordLabel.Name);
                Assert.AreEqual(testArtist.RecordLabel.TaxId, artist.RecordLabel.TaxId);
                Assert.AreEqual(testArtist.RecordLabel.Email, artist.RecordLabel.Email);
                Assert.AreEqual(testArtist.RecordLabel.Phone, artist.RecordLabel.Phone);
                Assert.IsNotNull(testArtist.RecordLabel.Address);
                Assert.AreEqual(testArtist.RecordLabel.Address.Street, artist.RecordLabel.Address.Street);
                Assert.AreEqual(testArtist.RecordLabel.Address.City, artist.RecordLabel.Address.City);
                Assert.AreEqual(testArtist.RecordLabel.Address.Region, artist.RecordLabel.Address.Region);
                Assert.AreEqual(testArtist.RecordLabel.Address.PostalCode, artist.RecordLabel.Address.PostalCode);
                Assert.IsNotNull(testArtist.RecordLabel.Address.Country);
                Assert.AreEqual(testArtist.RecordLabel.Address.Country.Name, artist.RecordLabel.Address.Country.Name);
                Assert.AreEqual(testArtist.RecordLabel.Address.Country.IsoCode, artist.RecordLabel.Address.Country.IsoCode);
            }

            var removeArtistTask = new RemoveArtist(DbContext);
            var removeResult = removeArtistTask.DoTask(artist);

            Assert.IsTrue(removeResult.Success);
            Assert.IsNull(removeResult.Exception);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new GetArtist(EmptyDbContext);
            var result = task.DoTask(0);
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
