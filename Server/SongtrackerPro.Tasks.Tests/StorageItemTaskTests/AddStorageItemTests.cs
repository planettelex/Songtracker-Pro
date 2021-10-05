using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Enums;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.ArtistTasks;
using SongtrackerPro.Tasks.LegalEntityTasks;
using SongtrackerPro.Tasks.PublishingTasks;
using SongtrackerPro.Tasks.RecordLabelTasks;
using SongtrackerPro.Tasks.StorageItemTasks;

namespace SongtrackerPro.Tasks.Tests.StorageItemTaskTests
{
    [TestClass]
    public class AddStorageItemTests : TestsBase
    {
        [TestMethod]
        public void TaskBaseSuccessTest()
        {
            var task = new AddStorageItem(DbContext);
            var testItem = TestsModel.StorageItem;
            var result = task.DoTask(testItem);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Data);

            var storageItemId = result.Data;
            Assert.IsNotNull(storageItemId);
            Assert.IsTrue(storageItemId.Value != Guid.Empty);

            var getStorageItemTask = new GetStorageItem(DbContext);
            var storageItem = getStorageItemTask.DoTask(storageItemId.Value)?.Data;

            Assert.IsNotNull(storageItem);
            Assert.AreEqual(testItem.Name, storageItem.Name);
            Assert.AreEqual(testItem.Container, storageItem.Container);
            Assert.AreEqual(testItem.FileName, storageItem.FileName);
            Assert.AreEqual(testItem.FolderPath, storageItem.FolderPath);

            var removeStorageItemTask = new RemoveStorageItem(DbContext);
            var removeStorageItemResult = removeStorageItemTask.DoTask(storageItem);

            Assert.IsTrue(removeStorageItemResult.Success);
            Assert.IsNull(removeStorageItemResult.Exception);
        }

        [TestMethod]
        public void TaskRecordLabelContractSuccessTest()
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
            var addRecordingTask = new AddRecording(DbContext);
            var addRecordingResult = addRecordingTask.DoTask(testRecording);

            Assert.IsTrue(addRecordingResult.Success);
            Assert.IsNull(addRecordingResult.Exception);
            Assert.IsNotNull(addRecordingResult.Data);

            var recordingId = addRecordingResult.Data;
            Assert.IsNotNull(recordingId);
            Assert.IsTrue(recordingId > 0);

            var testRelease = TestsModel.Release(testArtist, testRecordLabel);
            var addReleaseTask = new AddRelease(DbContext);
            var addReleaseResult = addReleaseTask.DoTask(testRelease);

            Assert.IsTrue(addReleaseResult.Success);
            Assert.IsNull(addReleaseResult.Exception);
            Assert.IsNotNull(addReleaseResult.Data);

            var releaseId = addReleaseResult.Data;
            Assert.IsNotNull(releaseId);
            Assert.IsTrue(releaseId > 0);

            var task = new AddStorageItem(DbContext);
            var testItem = TestsModel.RecordLabelContract(testRecording, testRelease);
            var result = task.DoTask(testItem);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Data);

            var storageItemId = result.Data;
            Assert.IsNotNull(storageItemId);
            Assert.IsTrue(storageItemId.Value != Guid.Empty);

            var getStorageItemTask = new GetStorageItem(DbContext);
            var recordLabelContract = getStorageItemTask.DoTask(storageItemId.Value)?.Data as RecordLabelContract;

            Assert.IsNotNull(recordLabelContract);
            Assert.AreEqual(testItem.Name, recordLabelContract.Name);
            Assert.AreEqual(testItem.Container, recordLabelContract.Container);
            Assert.AreEqual(testItem.FileName, recordLabelContract.FileName);
            Assert.AreEqual(testItem.FolderPath, recordLabelContract.FolderPath);
            Assert.AreEqual(testItem.PromiseePartyType, recordLabelContract.PromiseePartyType);
            Assert.AreEqual(testItem.PromisorPartyType, recordLabelContract.PromisorPartyType);
            Assert.AreEqual(DocumentType.Contract, recordLabelContract.DocumentType);
            Assert.IsNotNull(recordLabelContract.Release);
            Assert.AreEqual(testRelease.Title, recordLabelContract.Release.Title);
            Assert.AreEqual(testRelease.CatalogNumber, recordLabelContract.Release.CatalogNumber);
            Assert.IsNotNull(recordLabelContract.Release.Artist);
            Assert.AreEqual(testArtist.Name, recordLabelContract.Release.Artist.Name);
            Assert.AreEqual(testArtist.Email, recordLabelContract.Release.Artist.Email);
            Assert.IsNotNull(recordLabelContract.Release.RecordLabel);
            Assert.AreEqual(testRecordLabel.Name, recordLabelContract.Release.RecordLabel.Name);
            Assert.AreEqual(testRecordLabel.Email, recordLabelContract.Release.RecordLabel.Email);
            Assert.IsNotNull(recordLabelContract.Recording);
            Assert.AreEqual(testRecording.Title, recordLabelContract.Recording.Title);
            Assert.IsNotNull(recordLabelContract.Recording.Artist);
            Assert.AreEqual(testArtist.Name, recordLabelContract.Recording.Artist.Name);
            Assert.AreEqual(testArtist.Email, recordLabelContract.Recording.Artist.Email);
            Assert.IsNotNull(recordLabelContract.Recording.RecordLabel);
            Assert.AreEqual(testRecordLabel.Name, recordLabelContract.Recording.RecordLabel.Name);
            Assert.AreEqual(testRecordLabel.Email, recordLabelContract.Recording.RecordLabel.Email);

            var removeStorageItemTask = new RemoveStorageItem(DbContext);
            var removeStorageItemResult = removeStorageItemTask.DoTask(recordLabelContract);

            Assert.IsTrue(removeStorageItemResult.Success);
            Assert.IsNull(removeStorageItemResult.Exception);

            var removeRecordingTask = new RemoveRecording(DbContext);
            var removeRecordingResult = removeRecordingTask.DoTask(testRecording);

            Assert.IsTrue(removeRecordingResult.Success);
            Assert.IsNull(removeRecordingResult.Exception);

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
        public void TaskPublisherContractSuccessTest()
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

            var task = new AddStorageItem(DbContext);
            var testItem = TestsModel.PublisherContract(testPublication);
            var result = task.DoTask(testItem);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Data);

            var storageItemId = result.Data;
            Assert.IsNotNull(storageItemId);
            Assert.IsTrue(storageItemId.Value != Guid.Empty);

            var getStorageItemTask = new GetStorageItem(DbContext);
            var publisherContract = getStorageItemTask.DoTask(storageItemId.Value)?.Data as PublisherContract;

            Assert.IsNotNull(publisherContract);
            Assert.AreEqual(testItem.Name, publisherContract.Name);
            Assert.AreEqual(testItem.Container, publisherContract.Container);
            Assert.AreEqual(testItem.FileName, publisherContract.FileName);
            Assert.AreEqual(testItem.FolderPath, publisherContract.FolderPath);
            Assert.AreEqual(testItem.PromiseePartyType, publisherContract.PromiseePartyType);
            Assert.AreEqual(testItem.PromisorPartyType, publisherContract.PromisorPartyType);
            Assert.AreEqual(DocumentType.Contract, publisherContract.DocumentType);
            Assert.IsNotNull(publisherContract.Publication);
            Assert.AreEqual(testPublication.Title, publisherContract.Publication.Title);
            Assert.AreEqual(testPublication.CatalogNumber, publisherContract.Publication.CatalogNumber);
            Assert.IsNotNull(publisherContract.Publisher);
            Assert.AreEqual(testPublisher.Name, publisherContract.Publisher.Name);

            var removeStorageItemTask = new RemoveStorageItem(DbContext);
            var removeStorageItemResult = removeStorageItemTask.DoTask(publisherContract);

            Assert.IsTrue(removeStorageItemResult.Success);
            Assert.IsNull(removeStorageItemResult.Exception);

            var removePublisherTask = new RemovePublisher(DbContext);
            var removePublisherResult = removePublisherTask.DoTask(testPublisher);

            Assert.IsTrue(removePublisherResult.Success);
            Assert.IsNull(removePublisherResult.Exception);
        }

        [TestMethod]
        public void TaskContractSuccessTest()
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

            var testLegalEntity1 = TestsModel.LegalEntity;
            var addLegalEntityTask = new AddLegalEntity(DbContext, new FormattingService());
            var addLegalEntityResult = addLegalEntityTask.DoTask(testLegalEntity1);

            var legalEntity1Id = addLegalEntityResult.Data;
            Assert.IsNotNull(legalEntity1Id);
            Assert.IsTrue(legalEntity1Id > 0);

            var task = new AddStorageItem(DbContext);
            var testItem = TestsModel.Contract(testArtist);
            var testPromisor = new ContractParty
            {
                Role = ContractPartyRole.Promisor,
                LegalEntity = testLegalEntity1,
                IsPrincipal = true
            };
            testItem.Parties.Add(testPromisor);

            var testLegalEntity2 = TestsModel.LegalEntity;
            addLegalEntityTask = new AddLegalEntity(DbContext, new FormattingService());
            addLegalEntityResult = addLegalEntityTask.DoTask(testLegalEntity2);

            var legalEntity2Id = addLegalEntityResult.Data;
            Assert.IsNotNull(legalEntity2Id);
            Assert.IsTrue(legalEntity2Id > 0);
            var testPromisee = new ContractParty
            {
                Role = ContractPartyRole.Promisee,
                LegalEntity = testLegalEntity2,
                IsPrincipal = true
            };
            testItem.Parties.Add(testPromisee);
            var result = task.DoTask(testItem);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Data);

            var storageItemId = result.Data;
            Assert.IsNotNull(storageItemId);
            Assert.IsTrue(storageItemId.Value != Guid.Empty);

            var getStorageItemTask = new GetStorageItem(DbContext);
            var contract = getStorageItemTask.DoTask(storageItemId.Value)?.Data as Contract;

            Assert.IsNotNull(contract);
            Assert.AreEqual(testItem.Name, contract.Name);
            Assert.AreEqual(testItem.Container, contract.Container);
            Assert.AreEqual(testItem.FileName, contract.FileName);
            Assert.AreEqual(testItem.FolderPath, contract.FolderPath);
            Assert.AreEqual(testItem.PromiseePartyType, contract.PromiseePartyType);
            Assert.AreEqual(testItem.PromisorPartyType, contract.PromisorPartyType);
            Assert.AreEqual(DocumentType.Contract, contract.DocumentType);
            Assert.IsNotNull(contract.Artist);
            Assert.AreEqual(testArtist.Name, contract.Artist.Name);
            Assert.AreEqual(testArtist.Email, contract.Artist.Email);
            Assert.IsNotNull(contract.Parties);
            
            var contractPromisor = contract.Parties.SingleOrDefault(cp => cp.Id == testPromisor.Id);
            Assert.IsNotNull(contractPromisor);
            Assert.IsNotNull(contractPromisor.LegalEntity);
            Assert.AreEqual(testLegalEntity1.Name, contractPromisor.LegalEntity.Name);
            Assert.IsNotNull(contractPromisor.LegalEntity.Address);
            Assert.AreEqual(testLegalEntity1.Address.Street, contractPromisor.LegalEntity.Address.Street);
            Assert.IsNotNull(contractPromisor.LegalEntity.Address.Country);

            var contractPromisee = contract.Parties.SingleOrDefault(cp => cp.Id == testPromisee.Id);
            Assert.IsNotNull(contractPromisee);
            Assert.IsNotNull(contractPromisee.LegalEntity);
            Assert.AreEqual(testLegalEntity2.Name, contractPromisee.LegalEntity.Name);
            Assert.IsNotNull(contractPromisee.LegalEntity.Address);
            Assert.AreEqual(testLegalEntity2.Address.Street, contractPromisee.LegalEntity.Address.Street);
            Assert.IsNotNull(contractPromisee.LegalEntity.Address.Country);

            var removeStorageItemTask = new RemoveStorageItem(DbContext);
            var removeStorageItemResult = removeStorageItemTask.DoTask(contract);

            Assert.IsTrue(removeStorageItemResult.Success);
            Assert.IsNull(removeStorageItemResult.Exception);

            var removeArtistTask = new RemoveArtist(DbContext);
            var removeArtistResult = removeArtistTask.DoTask(testArtist);

            Assert.IsTrue(removeArtistResult.Success);
            Assert.IsNull(removeArtistResult.Exception);

            var removeLegalEntityTask = new RemoveLegalEntity(DbContext);
            var removeLegalEntityResult = removeLegalEntityTask.DoTask(testLegalEntity1);

            Assert.IsTrue(removeLegalEntityResult.Success);
            Assert.IsNull(removeLegalEntityResult.Exception);

            removeLegalEntityTask = new RemoveLegalEntity(DbContext);
            removeLegalEntityResult = removeLegalEntityTask.DoTask(testLegalEntity2);

            Assert.IsTrue(removeLegalEntityResult.Success);
            Assert.IsNull(removeLegalEntityResult.Exception);
        }

        [TestMethod]
        public void TaskDocumentSuccessTest()
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

            var task = new AddStorageItem(DbContext);
            var testItem = TestsModel.Document(testPublisher);
            var result = task.DoTask(testItem);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Data);

            var storageItemId = result.Data;
            Assert.IsNotNull(storageItemId);
            Assert.IsTrue(storageItemId.Value != Guid.Empty);

            var getStorageItemTask = new GetStorageItem(DbContext);
            var document = getStorageItemTask.DoTask(storageItemId.Value)?.Data as Document;

            Assert.IsNotNull(document);
            Assert.AreEqual(testItem.Name, document.Name);
            Assert.AreEqual(testItem.Container, document.Container);
            Assert.AreEqual(testItem.FileName, document.FileName);
            Assert.AreEqual(testItem.FolderPath, document.FolderPath);
            Assert.AreEqual(testItem.DocumentType, document.DocumentType);
            Assert.AreEqual(testItem.Version, document.Version);
            Assert.IsNotNull(document.Publisher);
            Assert.AreEqual(testPublisher.Name, document.Publisher.Name);
            Assert.AreEqual(testPublisher.Email, document.Publisher.Email);

            var removeStorageItemTask = new RemoveStorageItem(DbContext);
            var removeStorageItemResult = removeStorageItemTask.DoTask(document);

            Assert.IsTrue(removeStorageItemResult.Success);
            Assert.IsNull(removeStorageItemResult.Exception);

            var removePublisherTask = new RemovePublisher(DbContext);
            var removePublisherResult = removePublisherTask.DoTask(testPublisher);

            Assert.IsTrue(removePublisherResult.Success);
            Assert.IsNull(removePublisherResult.Exception);
        }

        [TestMethod]
        public void TaskDigitalMediaSuccessTest()
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

            var task = new AddStorageItem(DbContext);
            var testItem = TestsModel.DigitalMedia(testArtist, testRecordLabel);
            var result = task.DoTask(testItem);

            Assert.IsTrue(result.Success);
            Assert.IsNull(result.Exception);
            Assert.IsNotNull(result.Data);

            var storageItemId = result.Data;
            Assert.IsNotNull(storageItemId);
            Assert.IsTrue(storageItemId.Value != Guid.Empty);

            var getStorageItemTask = new GetStorageItem(DbContext);
            var digitalMedia = getStorageItemTask.DoTask(storageItemId.Value)?.Data as DigitalMedia;

            Assert.IsNotNull(digitalMedia);
            Assert.AreEqual(testItem.Name, digitalMedia.Name);
            Assert.AreEqual(testItem.Container, digitalMedia.Container);
            Assert.AreEqual(testItem.FileName, digitalMedia.FileName);
            Assert.AreEqual(testItem.FolderPath, digitalMedia.FolderPath);
            Assert.AreEqual(testItem.IsCompressed, digitalMedia.IsCompressed);
            Assert.AreEqual(testItem.MediaCategory, digitalMedia.MediaCategory);
            Assert.IsNotNull(digitalMedia.Artist);
            Assert.AreEqual(testArtist.Name, digitalMedia.Artist.Name);
            Assert.AreEqual(testArtist.Email, digitalMedia.Artist.Email);
            Assert.IsNotNull(digitalMedia.RecordLabel);
            Assert.AreEqual(testRecordLabel.Name, digitalMedia.RecordLabel.Name);
            Assert.AreEqual(testRecordLabel.Email, digitalMedia.RecordLabel.Email);

            var removeStorageItemTask = new RemoveStorageItem(DbContext);
            var removeStorageItemResult = removeStorageItemTask.DoTask(digitalMedia);

            Assert.IsTrue(removeStorageItemResult.Success);
            Assert.IsNull(removeStorageItemResult.Exception);

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
            var task = new AddStorageItem(EmptyDbContext);
            var result = task.DoTask(new StorageItem());
            
            Assert.IsFalse(result.Success);
            Assert.IsNotNull(result.Exception);
        }
    }
}
