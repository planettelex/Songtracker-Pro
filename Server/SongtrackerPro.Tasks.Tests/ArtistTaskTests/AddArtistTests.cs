using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Tasks.ArtistTasks;

namespace SongtrackerPro.Tasks.Tests.ArtistTaskTests
{
    [TestClass]
    public class AddArtistTests : TestsBase
    {
        [TestMethod]
        public void TaskSuccessTest()
        {
            var task = new AddArtist(DbContext);
            var testArtist = TestsModel.Artist;
            var result = task.DoTask(testArtist);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Data);

            var artistId = result.Data;
            Assert.IsNotNull(artistId);
            Assert.IsTrue(artistId > 0);

            var getArtistTask = new GetArtist(DbContext);
            var artist = getArtistTask.DoTask(artistId.Value)?.Data;

            Assert.IsNotNull(artist);
            Assert.AreEqual(testArtist.Name, artist.Name);
            Assert.AreEqual(testArtist.TaxId, artist.TaxId);
            Assert.AreEqual(testArtist.HasServiceMark, artist.HasServiceMark);
            Assert.AreEqual(testArtist.WebsiteUrl, artist.WebsiteUrl);
            Assert.AreEqual(testArtist.PressKitUrl, artist.PressKitUrl);
            if (testArtist.RecordLabel != null)
                Assert.AreEqual(testArtist.RecordLabel.Name, artist.RecordLabel.Name);

            var removeArtistTask = new RemoveArtist(DbContext);
            var removeResult = removeArtistTask.DoTask(artist);

            Assert.IsTrue(removeResult.Success);
            Assert.IsNull(removeResult.Exception);
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new AddArtist(EmptyDbContext);
            var result = task.DoTask(new Artist());
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
