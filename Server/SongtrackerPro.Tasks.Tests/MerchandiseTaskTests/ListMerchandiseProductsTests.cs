using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.ArtistTasks;
using SongtrackerPro.Tasks.MerchandiseTasks;
using SongtrackerPro.Tasks.PublishingTasks;
using SongtrackerPro.Tasks.RecordLabelTasks;

namespace SongtrackerPro.Tasks.Tests.MerchandiseTaskTests
{
    [TestClass]
    public class ListMerchandiseProductsTests : TestsBase
    {
        [TestMethod]
        public void TaskBaseSuccessTest()
        {
            var testMerchandiseItem = TestsModel.MerchandiseItem(null);
            var addMerchandiseItemTask = new AddMerchandiseItem(DbContext);
            var addMerchandiseItemResult = addMerchandiseItemTask.DoTask(testMerchandiseItem);

            Assert.IsTrue(addMerchandiseItemResult.Success);
            Assert.IsNull(addMerchandiseItemResult.Exception);

            var addMerchandiseProductTask = new AddMerchandiseProduct(DbContext);
            var testProduct1 = TestsModel.MerchandiseProduct(testMerchandiseItem);
            var addMerchandiseResult = addMerchandiseProductTask.DoTask(testProduct1);

            Assert.IsTrue(addMerchandiseResult.Success);
            Assert.IsNull(addMerchandiseResult.Exception);
            Assert.IsNotNull(addMerchandiseResult.Data);

            var merchandiseProduct1Id = addMerchandiseResult.Data;
            Assert.IsNotNull(merchandiseProduct1Id);
            Assert.IsTrue(merchandiseProduct1Id > 0);

            addMerchandiseProductTask = new AddMerchandiseProduct(DbContext);
            var testProduct2 = TestsModel.MerchandiseProduct(testMerchandiseItem);
            addMerchandiseResult = addMerchandiseProductTask.DoTask(testProduct2);

            Assert.IsTrue(addMerchandiseResult.Success);
            Assert.IsNull(addMerchandiseResult.Exception);
            Assert.IsNotNull(addMerchandiseResult.Data);

            var merchandiseProduct2Id = addMerchandiseResult.Data;
            Assert.IsNotNull(merchandiseProduct2Id);
            Assert.IsTrue(merchandiseProduct2Id > 0);

            var task = new ListMerchandiseProducts(DbContext);
            var result = task.DoTask(testMerchandiseItem);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);

            var merchandiseProducts = result.Data;
            Assert.IsNotNull(merchandiseProducts);
            Assert.IsTrue(merchandiseProducts.Count >= 2);

            var merchandiseProduct1 = merchandiseProducts.SingleOrDefault(mp => mp.Id == merchandiseProduct1Id);
            Assert.IsNotNull(merchandiseProduct1);
            Assert.AreEqual(testProduct1.Name, merchandiseProduct1.Name);
            Assert.AreEqual(testProduct1.Color, merchandiseProduct1.Color);
            Assert.AreEqual(testProduct1.ColorName, merchandiseProduct1.ColorName);
            Assert.AreEqual(testProduct1.Description, merchandiseProduct1.Description);
            Assert.AreEqual(testProduct1.Size, merchandiseProduct1.Size);
            Assert.AreEqual(testProduct1.Sku, merchandiseProduct1.Sku);
            Assert.AreEqual(testProduct1.Upc, merchandiseProduct1.Upc);
            Assert.IsNotNull(merchandiseProduct1.MerchandiseItem);
            Assert.AreEqual(testProduct1.MerchandiseItem.Name, merchandiseProduct1.MerchandiseItem.Name);
            Assert.AreEqual(testProduct1.MerchandiseItem.Description, merchandiseProduct1.MerchandiseItem.Description);
            Assert.AreEqual(testProduct1.MerchandiseItem.IsPromotional, merchandiseProduct1.MerchandiseItem.IsPromotional);
            Assert.IsNotNull(merchandiseProduct1.MerchandiseItem.Category);
            Assert.IsNotNull(merchandiseProduct1.MerchandiseItem.Category.Name);

            var merchandiseProduct2 = merchandiseProducts.SingleOrDefault(mp => mp.Id == merchandiseProduct2Id);
            Assert.IsNotNull(merchandiseProduct2);
            Assert.AreEqual(testProduct2.Name, merchandiseProduct2.Name);
            Assert.AreEqual(testProduct2.Color, merchandiseProduct2.Color);
            Assert.AreEqual(testProduct2.ColorName, merchandiseProduct2.ColorName);
            Assert.AreEqual(testProduct2.Description, merchandiseProduct2.Description);
            Assert.AreEqual(testProduct2.Size, merchandiseProduct2.Size);
            Assert.AreEqual(testProduct2.Sku, merchandiseProduct2.Sku);
            Assert.AreEqual(testProduct2.Upc, merchandiseProduct2.Upc);
            Assert.IsNotNull(merchandiseProduct2.MerchandiseItem);
            Assert.AreEqual(testProduct2.MerchandiseItem.Name, merchandiseProduct2.MerchandiseItem.Name);
            Assert.AreEqual(testProduct2.MerchandiseItem.Description, merchandiseProduct2.MerchandiseItem.Description);
            Assert.AreEqual(testProduct2.MerchandiseItem.IsPromotional, merchandiseProduct2.MerchandiseItem.IsPromotional);
            Assert.IsNotNull(merchandiseProduct2.MerchandiseItem.Category);
            Assert.IsNotNull(merchandiseProduct2.MerchandiseItem.Category.Name);

            var removeMerchandiseProductTask = new RemoveMerchandiseProduct(DbContext);
            var removeMerchandiseProductResult = removeMerchandiseProductTask.DoTask(merchandiseProduct1);

            Assert.IsTrue(removeMerchandiseProductResult.Success);
            Assert.IsNull(removeMerchandiseProductResult.Exception);

            removeMerchandiseProductTask = new RemoveMerchandiseProduct(DbContext);
            removeMerchandiseProductResult = removeMerchandiseProductTask.DoTask(merchandiseProduct2);

            Assert.IsTrue(removeMerchandiseProductResult.Success);
            Assert.IsNull(removeMerchandiseProductResult.Exception);

            var removeMerchandiseItemTask = new RemoveMerchandiseItem(DbContext);
            var removeMerchandiseItemResult = removeMerchandiseItemTask.DoTask(testMerchandiseItem);

            Assert.IsTrue(removeMerchandiseItemResult.Success);
            Assert.IsNull(removeMerchandiseItemResult.Exception);
        }

        [TestMethod]
        public void TaskPublicationSuccessTest()
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

            var testPublication = TestsModel.Publication(testPublisher);
            var addPublicationTask = new AddPublication(DbContext);
            var addPublicationResult = addPublicationTask.DoTask(testPublication);
            
            Assert.IsTrue(addPublicationResult.Success);
            Assert.IsNull(addPublicationResult.Exception);
            Assert.IsNotNull(addPublicationResult.Data);

            var testMerchandiseItem = TestsModel.MerchandiseItem(null);
            var addMerchandiseItemTask = new AddMerchandiseItem(DbContext);
            var addMerchandiseItemResult = addMerchandiseItemTask.DoTask(testMerchandiseItem);

            Assert.IsTrue(addMerchandiseItemResult.Success);
            Assert.IsNull(addMerchandiseItemResult.Exception);

            var addMerchandiseProductTask = new AddMerchandiseProduct(DbContext);
            var testProduct1 = TestsModel.PublicationMerchandiseProduct(testPublication, testMerchandiseItem);
            var addMerchandiseResult = addMerchandiseProductTask.DoTask(testProduct1);

            Assert.IsTrue(addMerchandiseResult.Success);
            Assert.IsNull(addMerchandiseResult.Exception);
            Assert.IsNotNull(addMerchandiseResult.Data);

            var merchandiseProduct1Id = addMerchandiseResult.Data;
            Assert.IsNotNull(merchandiseProduct1Id);
            Assert.IsTrue(merchandiseProduct1Id > 0);

            addMerchandiseProductTask = new AddMerchandiseProduct(DbContext);
            var testProduct2 = TestsModel.PublicationMerchandiseProduct(testPublication, testMerchandiseItem);
            addMerchandiseResult = addMerchandiseProductTask.DoTask(testProduct2);

            Assert.IsTrue(addMerchandiseResult.Success);
            Assert.IsNull(addMerchandiseResult.Exception);
            Assert.IsNotNull(addMerchandiseResult.Data);

            var merchandiseProduct2Id = addMerchandiseResult.Data;
            Assert.IsNotNull(merchandiseProduct2Id);
            Assert.IsTrue(merchandiseProduct2Id > 0);

            var task = new ListMerchandiseProducts(DbContext);
            var result = task.DoTask(testMerchandiseItem);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);

            var merchandiseProducts = result.Data;
            Assert.IsNotNull(merchandiseProducts);
            Assert.IsTrue(merchandiseProducts.Count >= 2);

            var merchandiseProduct1 = merchandiseProducts.SingleOrDefault(mp => mp.Id == merchandiseProduct1Id) as PublicationMerchandiseProduct;
            Assert.IsNotNull(merchandiseProduct1);
            Assert.AreEqual(testProduct1.Name, merchandiseProduct1.Name);
            Assert.AreEqual(testProduct1.Color, merchandiseProduct1.Color);
            Assert.AreEqual(testProduct1.ColorName, merchandiseProduct1.ColorName);
            Assert.AreEqual(testProduct1.Description, merchandiseProduct1.Description);
            Assert.AreEqual(testProduct1.Size, merchandiseProduct1.Size);
            Assert.AreEqual(testProduct1.Sku, merchandiseProduct1.Sku);
            Assert.AreEqual(testProduct1.Upc, merchandiseProduct1.Upc);
            Assert.AreEqual(testProduct1.IssueNumber, merchandiseProduct1.IssueNumber);
            Assert.IsNotNull(merchandiseProduct1.Publication);
            Assert.AreEqual(testPublication.Title, merchandiseProduct1.Publication.Title);
            Assert.AreEqual(testPublication.CatalogNumber, merchandiseProduct1.Publication.CatalogNumber);
            Assert.AreEqual(testPublication.Isbn, merchandiseProduct1.Publication.Isbn);
            Assert.IsNotNull(merchandiseProduct1.Publication.Publisher);
            Assert.AreEqual(testPublisher.Name, merchandiseProduct1.Publication.Publisher.Name);
            Assert.AreEqual(testPublisher.Email, merchandiseProduct1.Publication.Publisher.Email);
            Assert.IsNotNull(merchandiseProduct1.MerchandiseItem);
            Assert.AreEqual(testProduct1.MerchandiseItem.Name, merchandiseProduct1.MerchandiseItem.Name);
            Assert.AreEqual(testProduct1.MerchandiseItem.Description, merchandiseProduct1.MerchandiseItem.Description);
            Assert.AreEqual(testProduct1.MerchandiseItem.IsPromotional, merchandiseProduct1.MerchandiseItem.IsPromotional);
            Assert.IsNotNull(merchandiseProduct1.MerchandiseItem.Category);
            Assert.IsNotNull(merchandiseProduct1.MerchandiseItem.Category.Name);

            var merchandiseProduct2 = merchandiseProducts.SingleOrDefault(mp => mp.Id == merchandiseProduct2Id) as PublicationMerchandiseProduct;
            Assert.IsNotNull(merchandiseProduct2);
            Assert.AreEqual(testProduct2.Name, merchandiseProduct2.Name);
            Assert.AreEqual(testProduct2.Color, merchandiseProduct2.Color);
            Assert.AreEqual(testProduct2.ColorName, merchandiseProduct2.ColorName);
            Assert.AreEqual(testProduct2.Description, merchandiseProduct2.Description);
            Assert.AreEqual(testProduct2.Size, merchandiseProduct2.Size);
            Assert.AreEqual(testProduct2.Sku, merchandiseProduct2.Sku);
            Assert.AreEqual(testProduct2.Upc, merchandiseProduct2.Upc);
            Assert.AreEqual(testProduct2.IssueNumber, merchandiseProduct2.IssueNumber);
            Assert.IsNotNull(merchandiseProduct2.Publication);
            Assert.AreEqual(testPublication.Title, merchandiseProduct2.Publication.Title);
            Assert.AreEqual(testPublication.CatalogNumber, merchandiseProduct2.Publication.CatalogNumber);
            Assert.AreEqual(testPublication.Isbn, merchandiseProduct2.Publication.Isbn);
            Assert.IsNotNull(merchandiseProduct2.Publication.Publisher);
            Assert.AreEqual(testPublisher.Name, merchandiseProduct2.Publication.Publisher.Name);
            Assert.AreEqual(testPublisher.Email, merchandiseProduct2.Publication.Publisher.Email);
            Assert.IsNotNull(merchandiseProduct2.MerchandiseItem);
            Assert.AreEqual(testProduct2.MerchandiseItem.Name, merchandiseProduct2.MerchandiseItem.Name);
            Assert.AreEqual(testProduct2.MerchandiseItem.Description, merchandiseProduct2.MerchandiseItem.Description);
            Assert.AreEqual(testProduct2.MerchandiseItem.IsPromotional, merchandiseProduct2.MerchandiseItem.IsPromotional);
            Assert.IsNotNull(merchandiseProduct2.MerchandiseItem.Category);
            Assert.IsNotNull(merchandiseProduct2.MerchandiseItem.Category.Name);

            var removeMerchandiseProductTask = new RemoveMerchandiseProduct(DbContext);
            var removeMerchandiseProductResult = removeMerchandiseProductTask.DoTask(merchandiseProduct1);

            Assert.IsTrue(removeMerchandiseProductResult.Success);
            Assert.IsNull(removeMerchandiseProductResult.Exception);

            removeMerchandiseProductTask = new RemoveMerchandiseProduct(DbContext);
            removeMerchandiseProductResult = removeMerchandiseProductTask.DoTask(merchandiseProduct2);

            Assert.IsTrue(removeMerchandiseProductResult.Success);
            Assert.IsNull(removeMerchandiseProductResult.Exception);

            var removeMerchandiseItemTask = new RemoveMerchandiseItem(DbContext);
            var removeMerchandiseItemResult = removeMerchandiseItemTask.DoTask(testMerchandiseItem);

            Assert.IsTrue(removeMerchandiseItemResult.Success);
            Assert.IsNull(removeMerchandiseItemResult.Exception);

            var removePublisherTask = new RemovePublisher(DbContext);
            var removePublisherResult = removePublisherTask.DoTask(testPublisher);

            Assert.IsTrue(removePublisherResult.Success);
            Assert.IsNull(removePublisherResult.Exception);
        }

        [TestMethod]
        public void TaskReleaseSuccessTest()
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
            var addReleaseTask = new AddRelease(DbContext);
            var addReleaseResult = addReleaseTask.DoTask(testRelease);

            Assert.IsTrue(addReleaseResult.Success);
            Assert.IsNull(addReleaseResult.Exception);
            Assert.IsNotNull(addReleaseResult.Data);

            var releaseId = addReleaseResult.Data;
            Assert.IsNotNull(releaseId);
            Assert.IsTrue(releaseId > 0);

            var testMerchandiseItem = TestsModel.MerchandiseItem(null);
            var addMerchandiseItemTask = new AddMerchandiseItem(DbContext);
            var addMerchandiseItemResult = addMerchandiseItemTask.DoTask(testMerchandiseItem);

            Assert.IsTrue(addMerchandiseItemResult.Success);
            Assert.IsNull(addMerchandiseItemResult.Exception);

            var addMerchandiseProductTask = new AddMerchandiseProduct(DbContext);
            var testProduct1 = TestsModel.ReleaseMerchandiseProduct(testRelease, testMerchandiseItem);
            var addMerchandiseResult = addMerchandiseProductTask.DoTask(testProduct1);

            Assert.IsTrue(addMerchandiseResult.Success);
            Assert.IsNull(addMerchandiseResult.Exception);
            Assert.IsNotNull(addMerchandiseResult.Data);

            var merchandiseProduct1Id = addMerchandiseResult.Data;
            Assert.IsNotNull(merchandiseProduct1Id);
            Assert.IsTrue(merchandiseProduct1Id > 0);

            addMerchandiseProductTask = new AddMerchandiseProduct(DbContext);
            var testProduct2 = TestsModel.ReleaseMerchandiseProduct(testRelease, testMerchandiseItem);
            addMerchandiseResult = addMerchandiseProductTask.DoTask(testProduct2);

            Assert.IsTrue(addMerchandiseResult.Success);
            Assert.IsNull(addMerchandiseResult.Exception);
            Assert.IsNotNull(addMerchandiseResult.Data);

            var merchandiseProduct2Id = addMerchandiseResult.Data;
            Assert.IsNotNull(merchandiseProduct2Id);
            Assert.IsTrue(merchandiseProduct2Id > 0);

            var task = new ListMerchandiseProducts(DbContext);
            var result = task.DoTask(testMerchandiseItem);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);

            var merchandiseProducts = result.Data;
            Assert.IsNotNull(merchandiseProducts);
            Assert.IsTrue(merchandiseProducts.Count >= 2);

            var merchandiseProduct1 = merchandiseProducts.SingleOrDefault(mp => mp.Id == merchandiseProduct1Id) as ReleaseMerchandiseProduct;
            Assert.IsNotNull(merchandiseProduct1);
            Assert.AreEqual(testProduct1.Name, merchandiseProduct1.Name);
            Assert.AreEqual(testProduct1.Color, merchandiseProduct1.Color);
            Assert.AreEqual(testProduct1.ColorName, merchandiseProduct1.ColorName);
            Assert.AreEqual(testProduct1.Description, merchandiseProduct1.Description);
            Assert.AreEqual(testProduct1.Size, merchandiseProduct1.Size);
            Assert.AreEqual(testProduct1.Sku, merchandiseProduct1.Sku);
            Assert.AreEqual(testProduct1.Upc, merchandiseProduct1.Upc);
            Assert.AreEqual(testProduct1.MediaType, merchandiseProduct1.MediaType);
            Assert.IsNotNull(merchandiseProduct1.Release);
            Assert.AreEqual(testRelease.Title, merchandiseProduct1.Release.Title);
            Assert.AreEqual(testRelease.Type, merchandiseProduct1.Release.Type);
            Assert.AreEqual(testRelease.CatalogNumber, merchandiseProduct1.Release.CatalogNumber);
            Assert.IsNotNull(merchandiseProduct1.Release.Artist);
            Assert.AreEqual(testArtist.Name, merchandiseProduct1.Release.Artist.Name);
            Assert.AreEqual(testArtist.Email, merchandiseProduct1.Release.Artist.Email);
            Assert.IsNotNull(merchandiseProduct1.Release.Genre);
            Assert.IsNotNull(merchandiseProduct1.Release.RecordLabel);
            Assert.AreEqual(testRecordLabel.Name, merchandiseProduct1.Release.RecordLabel.Name);
            Assert.AreEqual(testRecordLabel.Email, merchandiseProduct1.Release.RecordLabel.Email);
            Assert.IsNotNull(merchandiseProduct1.MerchandiseItem);
            Assert.AreEqual(testMerchandiseItem.Name, merchandiseProduct1.MerchandiseItem.Name);
            Assert.AreEqual(testMerchandiseItem.Description, merchandiseProduct1.MerchandiseItem.Description);
            Assert.AreEqual(testMerchandiseItem.IsPromotional, merchandiseProduct1.MerchandiseItem.IsPromotional);
            Assert.IsNotNull(merchandiseProduct1.MerchandiseItem.Category);
            Assert.IsNotNull(merchandiseProduct1.MerchandiseItem.Category.Name);

            var merchandiseProduct2 = merchandiseProducts.SingleOrDefault(mp => mp.Id == merchandiseProduct2Id) as ReleaseMerchandiseProduct;
            Assert.IsNotNull(merchandiseProduct2);
            Assert.AreEqual(testProduct2.Name, merchandiseProduct2.Name);
            Assert.AreEqual(testProduct2.Color, merchandiseProduct2.Color);
            Assert.AreEqual(testProduct2.ColorName, merchandiseProduct2.ColorName);
            Assert.AreEqual(testProduct2.Description, merchandiseProduct2.Description);
            Assert.AreEqual(testProduct2.Size, merchandiseProduct2.Size);
            Assert.AreEqual(testProduct2.Sku, merchandiseProduct2.Sku);
            Assert.AreEqual(testProduct2.Upc, merchandiseProduct2.Upc);
            Assert.AreEqual(testProduct2.MediaType, merchandiseProduct2.MediaType);
            Assert.IsNotNull(merchandiseProduct2.Release);
            Assert.AreEqual(testRelease.Title, merchandiseProduct2.Release.Title);
            Assert.AreEqual(testRelease.Type, merchandiseProduct2.Release.Type);
            Assert.AreEqual(testRelease.CatalogNumber, merchandiseProduct2.Release.CatalogNumber);
            Assert.IsNotNull(merchandiseProduct2.Release.Artist);
            Assert.AreEqual(testArtist.Name, merchandiseProduct2.Release.Artist.Name);
            Assert.AreEqual(testArtist.Email, merchandiseProduct2.Release.Artist.Email);
            Assert.IsNotNull(merchandiseProduct2.Release.Genre);
            Assert.IsNotNull(merchandiseProduct2.Release.RecordLabel);
            Assert.AreEqual(testRecordLabel.Name, merchandiseProduct2.Release.RecordLabel.Name);
            Assert.AreEqual(testRecordLabel.Email, merchandiseProduct2.Release.RecordLabel.Email);
            Assert.IsNotNull(merchandiseProduct2.MerchandiseItem);
            Assert.AreEqual(testMerchandiseItem.Name, merchandiseProduct2.MerchandiseItem.Name);
            Assert.AreEqual(testMerchandiseItem.Description, merchandiseProduct2.MerchandiseItem.Description);
            Assert.AreEqual(testMerchandiseItem.IsPromotional, merchandiseProduct2.MerchandiseItem.IsPromotional);
            Assert.IsNotNull(merchandiseProduct2.MerchandiseItem.Category);
            Assert.IsNotNull(merchandiseProduct2.MerchandiseItem.Category.Name);

            var removeMerchandiseProductTask = new RemoveMerchandiseProduct(DbContext);
            var removeMerchandiseProductResult = removeMerchandiseProductTask.DoTask(merchandiseProduct1);

            Assert.IsTrue(removeMerchandiseProductResult.Success);
            Assert.IsNull(removeMerchandiseProductResult.Exception);

            removeMerchandiseProductTask = new RemoveMerchandiseProduct(DbContext);
            removeMerchandiseProductResult = removeMerchandiseProductTask.DoTask(merchandiseProduct2);

            Assert.IsTrue(removeMerchandiseProductResult.Success);
            Assert.IsNull(removeMerchandiseProductResult.Exception);

            var removeMerchandiseItemTask = new RemoveMerchandiseItem(DbContext);
            var removeMerchandiseItemResult = removeMerchandiseItemTask.DoTask(testMerchandiseItem);

            Assert.IsTrue(removeMerchandiseItemResult.Success);
            Assert.IsNull(removeMerchandiseItemResult.Exception);

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
        }

        [TestMethod]
        public void TaskFailTest()
        {
            var task = new ListMerchandiseProducts(EmptyDbContext);
            var result = task.DoTask(new MerchandiseItem());
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
