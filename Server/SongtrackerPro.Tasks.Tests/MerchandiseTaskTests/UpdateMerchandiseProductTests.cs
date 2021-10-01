using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Enums;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.ArtistTasks;
using SongtrackerPro.Tasks.MerchandiseTasks;
using SongtrackerPro.Tasks.PublishingTasks;
using SongtrackerPro.Tasks.RecordLabelTasks;

namespace SongtrackerPro.Tasks.Tests.MerchandiseTaskTests
{
    [TestClass]
    public class UpdateMerchandiseProductTests : TestsBase
    {
        private void UpdateMerchandiseProductModel(MerchandiseProduct merchandiseProduct)
        {
            var stamp = DateTime.Now.Ticks;
            var (colorName, hexValue) = TestsModel.Color;
            merchandiseProduct.ColorName = colorName;
            merchandiseProduct.Color = hexValue;
            merchandiseProduct.Name = "Update " + stamp;
            merchandiseProduct.Description = "Description " + stamp;
            merchandiseProduct.Size = TestsModel.Size;
            merchandiseProduct.Sku = "Sku " + " " + stamp;
            merchandiseProduct.Upc = "Upc " + " " + stamp;
        }

        private void UpdatePublicationProductModel(PublicationMerchandiseProduct merchandiseProduct)
        {
            UpdateMerchandiseProductModel(merchandiseProduct);
            var oneToOneHundred = new Random().Next(1, 100);
            merchandiseProduct.IssueNumber = "#" + oneToOneHundred;
        }

        private void UpdateReleaseProductModel(ReleaseMerchandiseProduct merchandiseProduct)
        {
            UpdateMerchandiseProductModel(merchandiseProduct);
            var oneToTwelve = new Random().Next(1, 12);
            merchandiseProduct.MediaType = (MediaType)oneToTwelve;
        }

        [TestMethod]
        public void TaskBaseSuccessTest()
        {
            var testMerchandiseItem = TestsModel.MerchandiseItem(null);
            var addMerchandiseItemTask = new AddMerchandiseItem(DbContext);
            var addMerchandiseItemResult = addMerchandiseItemTask.DoTask(testMerchandiseItem);

            Assert.IsTrue(addMerchandiseItemResult.Success);
            Assert.IsNull(addMerchandiseItemResult.Exception);

            var addMerchandiseProductTask = new AddMerchandiseProduct(DbContext);
            var testProduct = TestsModel.MerchandiseProduct(testMerchandiseItem);
            var addMerchandiseResult = addMerchandiseProductTask.DoTask(testProduct);

            Assert.IsTrue(addMerchandiseResult.Success);
            Assert.IsNull(addMerchandiseResult.Exception);
            Assert.IsNotNull(addMerchandiseResult.Data);

            var merchandiseProductId = addMerchandiseResult.Data;
            Assert.IsNotNull(merchandiseProductId);
            Assert.IsTrue(merchandiseProductId > 0);

            var task = new UpdateMerchandiseProduct(DbContext);
            UpdateMerchandiseProductModel(testProduct);
            var result = task.DoTask(testProduct);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNull(result.Data);

            var getMerchandiseProductTask = new GetMerchandiseProduct(DbContext);
            var merchandiseProduct = getMerchandiseProductTask.DoTask(merchandiseProductId.Value)?.Data;

            Assert.IsNotNull(merchandiseProduct);
            Assert.AreEqual(testProduct.Name, merchandiseProduct.Name);
            Assert.AreEqual(testProduct.Color, merchandiseProduct.Color);
            Assert.AreEqual(testProduct.ColorName, merchandiseProduct.ColorName);
            Assert.AreEqual(testProduct.Description, merchandiseProduct.Description);
            Assert.AreEqual(testProduct.Size, merchandiseProduct.Size);
            Assert.AreEqual(testProduct.Sku, merchandiseProduct.Sku);
            Assert.AreEqual(testProduct.Upc, merchandiseProduct.Upc);
            Assert.IsNotNull(merchandiseProduct.MerchandiseItem);
            Assert.AreEqual(testMerchandiseItem.Name, merchandiseProduct.MerchandiseItem.Name);
            Assert.AreEqual(testMerchandiseItem.Description, merchandiseProduct.MerchandiseItem.Description);
            Assert.AreEqual(testMerchandiseItem.IsPromotional, merchandiseProduct.MerchandiseItem.IsPromotional);
            Assert.IsNotNull(merchandiseProduct.MerchandiseItem.Category);
            Assert.IsNotNull(merchandiseProduct.MerchandiseItem.Category.Name);

            var removeMerchandiseProductTask = new RemoveMerchandiseProduct(DbContext);
            var removeMerchandiseProductResult = removeMerchandiseProductTask.DoTask(merchandiseProduct);

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

            var publicationId = addPublicationResult.Data;
            Assert.IsNotNull(publicationId);
            Assert.IsTrue(publicationId > 0);

            var testMerchandiseItem = TestsModel.MerchandiseItem(null);
            var addMerchandiseItemTask = new AddMerchandiseItem(DbContext);
            var addMerchandiseItemResult = addMerchandiseItemTask.DoTask(testMerchandiseItem);

            Assert.IsTrue(addMerchandiseItemResult.Success);
            Assert.IsNull(addMerchandiseItemResult.Exception);

            var addMerchandiseProductTask = new AddMerchandiseProduct(DbContext);
            var testProduct = TestsModel.PublicationMerchandiseProduct(testPublication, testMerchandiseItem);
            var addMerchandiseProductResult = addMerchandiseProductTask.DoTask(testProduct);

            Assert.IsTrue(addMerchandiseProductResult.Success);
            Assert.IsNull(addMerchandiseProductResult.Exception);
            Assert.IsNotNull(addMerchandiseProductResult.Data);

            var merchandiseProductId = addMerchandiseProductResult.Data;
            Assert.IsNotNull(merchandiseProductId);
            Assert.IsTrue(merchandiseProductId > 0);

            var task = new UpdateMerchandiseProduct(DbContext);
            UpdatePublicationProductModel(testProduct);
            var result = task.DoTask(testProduct);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNull(result.Data);

            var getMerchandiseProductTask = new GetMerchandiseProduct(DbContext);
            var merchandiseProduct = getMerchandiseProductTask.DoTask(merchandiseProductId.Value)?.Data as PublicationMerchandiseProduct;

            Assert.IsNotNull(merchandiseProduct);
            Assert.AreEqual(testProduct.Name, merchandiseProduct.Name);
            Assert.AreEqual(testProduct.Color, merchandiseProduct.Color);
            Assert.AreEqual(testProduct.ColorName, merchandiseProduct.ColorName);
            Assert.AreEqual(testProduct.Description, merchandiseProduct.Description);
            Assert.AreEqual(testProduct.Size, merchandiseProduct.Size);
            Assert.AreEqual(testProduct.Sku, merchandiseProduct.Sku);
            Assert.AreEqual(testProduct.Upc, merchandiseProduct.Upc);
            Assert.AreEqual(testProduct.IssueNumber, merchandiseProduct.IssueNumber);
            Assert.IsNotNull(merchandiseProduct.Publication);
            Assert.AreEqual(testPublication.Title, merchandiseProduct.Publication.Title);
            Assert.AreEqual(testPublication.CatalogNumber, merchandiseProduct.Publication.CatalogNumber);
            Assert.AreEqual(testPublication.Isbn, merchandiseProduct.Publication.Isbn);
            Assert.IsNotNull(merchandiseProduct.Publication.Publisher);
            Assert.AreEqual(testPublisher.Name, merchandiseProduct.Publication.Publisher.Name);
            Assert.AreEqual(testPublisher.Email, merchandiseProduct.Publication.Publisher.Email);
            Assert.IsNotNull(merchandiseProduct.MerchandiseItem);
            Assert.AreEqual(testMerchandiseItem.Name, merchandiseProduct.MerchandiseItem.Name);
            Assert.AreEqual(testMerchandiseItem.Description, merchandiseProduct.MerchandiseItem.Description);
            Assert.AreEqual(testMerchandiseItem.IsPromotional, merchandiseProduct.MerchandiseItem.IsPromotional);
            Assert.IsNotNull(merchandiseProduct.MerchandiseItem.Category);
            Assert.IsNotNull(merchandiseProduct.MerchandiseItem.Category.Name);

            var removeMerchandiseProductTask = new RemoveMerchandiseProduct(DbContext);
            var removeMerchandiseProductResult = removeMerchandiseProductTask.DoTask(merchandiseProduct);

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
            var testProduct = TestsModel.ReleaseMerchandiseProduct(testRelease, testMerchandiseItem);
            var addMerchandiseProductResult = addMerchandiseProductTask.DoTask(testProduct);

            Assert.IsTrue(addMerchandiseProductResult.Success);
            Assert.IsNull(addMerchandiseProductResult.Exception);
            Assert.IsNotNull(addMerchandiseProductResult.Data);

            var merchandiseProductId = addMerchandiseProductResult.Data;
            Assert.IsNotNull(merchandiseProductId);
            Assert.IsTrue(merchandiseProductId > 0);

            var task = new UpdateMerchandiseProduct(DbContext);
            UpdateReleaseProductModel(testProduct);
            var result = task.DoTask(testProduct);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNull(result.Data);

            var getMerchandiseProductTask = new GetMerchandiseProduct(DbContext);
            var merchandiseProduct = getMerchandiseProductTask.DoTask(merchandiseProductId.Value)?.Data as ReleaseMerchandiseProduct;

            Assert.IsNotNull(merchandiseProduct);
            Assert.AreEqual(testProduct.Name, merchandiseProduct.Name);
            Assert.AreEqual(testProduct.Color, merchandiseProduct.Color);
            Assert.AreEqual(testProduct.ColorName, merchandiseProduct.ColorName);
            Assert.AreEqual(testProduct.Description, merchandiseProduct.Description);
            Assert.AreEqual(testProduct.Size, merchandiseProduct.Size);
            Assert.AreEqual(testProduct.Sku, merchandiseProduct.Sku);
            Assert.AreEqual(testProduct.Upc, merchandiseProduct.Upc);
            Assert.AreEqual(testProduct.MediaType, merchandiseProduct.MediaType);
            Assert.IsNotNull(merchandiseProduct.Release);
            Assert.AreEqual(testRelease.Title, merchandiseProduct.Release.Title);
            Assert.AreEqual(testRelease.Type, merchandiseProduct.Release.Type);
            Assert.AreEqual(testRelease.CatalogNumber, merchandiseProduct.Release.CatalogNumber);
            Assert.IsNotNull(merchandiseProduct.Release.Artist);
            Assert.AreEqual(testArtist.Name, merchandiseProduct.Release.Artist.Name);
            Assert.AreEqual(testArtist.Email, merchandiseProduct.Release.Artist.Email);
            Assert.IsNotNull(merchandiseProduct.Release.Genre);
            Assert.IsNotNull(merchandiseProduct.Release.RecordLabel);
            Assert.AreEqual(testRecordLabel.Name, merchandiseProduct.Release.RecordLabel.Name);
            Assert.AreEqual(testRecordLabel.Email, merchandiseProduct.Release.RecordLabel.Email);
            Assert.IsNotNull(merchandiseProduct.MerchandiseItem);
            Assert.AreEqual(testMerchandiseItem.Name, merchandiseProduct.MerchandiseItem.Name);
            Assert.AreEqual(testMerchandiseItem.Description, merchandiseProduct.MerchandiseItem.Description);
            Assert.AreEqual(testMerchandiseItem.IsPromotional, merchandiseProduct.MerchandiseItem.IsPromotional);
            Assert.IsNotNull(merchandiseProduct.MerchandiseItem.Category);
            Assert.IsNotNull(merchandiseProduct.MerchandiseItem.Category.Name);

            var removeMerchandiseProductTask = new RemoveMerchandiseProduct(DbContext);
            var removeMerchandiseProductResult = removeMerchandiseProductTask.DoTask(merchandiseProduct);

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
            var task = new UpdateMerchandiseProduct(EmptyDbContext);
            var result = task.DoTask(new MerchandiseProduct());
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
