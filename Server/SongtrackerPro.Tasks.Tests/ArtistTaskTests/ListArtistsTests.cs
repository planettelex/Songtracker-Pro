using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Tasks.ArtistTasks;

namespace SongtrackerPro.Tasks.Tests.ArtistTaskTests
{
    [TestClass]
    public class ListArtistsTests : TestsBase
    {
        [TestMethod]
        public void TaskSuccessTest()
        {
            var addArtist = new AddArtist(DbContext);
            var testArtist1 = TestModel.Artist;
            var testArtist1Id = addArtist.DoTask(testArtist1);
            Assert.IsTrue(testArtist1Id.Data.HasValue);
            var testArtist2 = TestModel.Artist;
            var testArtist2Id = addArtist.DoTask(testArtist2);
            Assert.IsTrue(testArtist2Id.Data.HasValue);
            
            var task = new ListArtists(DbContext);
            var result = task.DoTask(null);
            
            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);

            var artists = result.Data;
            Assert.IsNotNull(artists);
            Assert.IsTrue(artists.Count >= 2);

            var artist1 = artists.SingleOrDefault(a => a.Id == testArtist1Id.Data.Value);
            Assert.IsNotNull(artist1);
            Assert.AreEqual(testArtist1.Name, artist1.Name);
            Assert.AreEqual(testArtist1.TaxId, artist1.TaxId);
            Assert.AreEqual(testArtist1.HasServiceMark, artist1.HasServiceMark);
            Assert.AreEqual(testArtist1.WebsiteUrl, artist1.WebsiteUrl);
            Assert.AreEqual(testArtist1.PressKitUrl, artist1.PressKitUrl);
            if (testArtist1.RecordLabel != null)
            {
                Assert.AreEqual(testArtist1.RecordLabel.Name, artist1.RecordLabel.Name);
                Assert.AreEqual(testArtist1.RecordLabel.TaxId, artist1.RecordLabel.TaxId);
                Assert.AreEqual(testArtist1.RecordLabel.Email, artist1.RecordLabel.Email);
                Assert.AreEqual(testArtist1.RecordLabel.Phone, artist1.RecordLabel.Phone);
                Assert.IsNotNull(testArtist1.RecordLabel.Address);
                Assert.AreEqual(testArtist1.RecordLabel.Address.Street, artist1.RecordLabel.Address.Street);
                Assert.AreEqual(testArtist1.RecordLabel.Address.City, artist1.RecordLabel.Address.City);
                Assert.AreEqual(testArtist1.RecordLabel.Address.Region, artist1.RecordLabel.Address.Region);
                Assert.AreEqual(testArtist1.RecordLabel.Address.PostalCode, artist1.RecordLabel.Address.PostalCode);
                Assert.IsNotNull(testArtist1.RecordLabel.Address.Country);
                Assert.AreEqual(testArtist1.RecordLabel.Address.Country.Name, artist1.RecordLabel.Address.Country.Name);
                Assert.AreEqual(testArtist1.RecordLabel.Address.Country.IsoCode, artist1.RecordLabel.Address.Country.IsoCode);
            }

            var artist2 = artists.SingleOrDefault(a => a.Id == testArtist2Id.Data.Value);
            Assert.IsNotNull(artist2);
            Assert.AreEqual(testArtist2.Name, artist2.Name);
            Assert.AreEqual(testArtist2.TaxId, artist2.TaxId);
            Assert.AreEqual(testArtist2.HasServiceMark, artist2.HasServiceMark);
            Assert.AreEqual(testArtist2.WebsiteUrl, artist2.WebsiteUrl);
            Assert.AreEqual(testArtist2.PressKitUrl, artist2.PressKitUrl);
            if (testArtist2.RecordLabel != null)
            {
                Assert.AreEqual(testArtist2.RecordLabel.Name, artist2.RecordLabel.Name);
                Assert.AreEqual(testArtist2.RecordLabel.TaxId, artist2.RecordLabel.TaxId);
                Assert.AreEqual(testArtist2.RecordLabel.Email, artist2.RecordLabel.Email);
                Assert.AreEqual(testArtist2.RecordLabel.Phone, artist2.RecordLabel.Phone);
                Assert.IsNotNull(testArtist2.RecordLabel.Address);
                Assert.AreEqual(testArtist2.RecordLabel.Address.Street, artist2.RecordLabel.Address.Street);
                Assert.AreEqual(testArtist2.RecordLabel.Address.City, artist2.RecordLabel.Address.City);
                Assert.AreEqual(testArtist2.RecordLabel.Address.Region, artist2.RecordLabel.Address.Region);
                Assert.AreEqual(testArtist2.RecordLabel.Address.PostalCode, artist2.RecordLabel.Address.PostalCode);
                Assert.IsNotNull(testArtist2.RecordLabel.Address.Country);
                Assert.AreEqual(testArtist2.RecordLabel.Address.Country.Name, artist2.RecordLabel.Address.Country.Name);
                Assert.AreEqual(testArtist2.RecordLabel.Address.Country.IsoCode, artist2.RecordLabel.Address.Country.IsoCode);
            }

            var removeArtist = new RemoveArtist(DbContext);
            var removeResult1 = removeArtist.DoTask(artist1);
            var removeResult2 = removeArtist.DoTask(artist2);

            Assert.IsTrue(removeResult1.Success);
            Assert.IsNull(removeResult1.Exception);

            Assert.IsTrue(removeResult2.Success);
            Assert.IsNull(removeResult2.Exception);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new ListArtists(EmptyDbContext);
            var result = task.DoTask(null);
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
