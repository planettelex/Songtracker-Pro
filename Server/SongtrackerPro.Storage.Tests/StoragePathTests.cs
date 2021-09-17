using Microsoft.VisualStudio.TestTools.UnitTesting;
using SongtrackerPro.Data.Enums;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Storage.Tests
{
    [TestClass]
    public class StoragePathTests
    {
        [TestMethod]
        public void GetRootFoldersTests()
        {
            var publisher = new Publisher();
            var rootFolders = StoragePath.RootFolders(publisher);
            Assert.IsNotNull(rootFolders);
            Assert.AreEqual("images/", rootFolders[0]);
            Assert.AreEqual("pamphlets/", rootFolders[1]);
            Assert.AreEqual("contracts/", rootFolders[2]);
            Assert.AreEqual("publications/", rootFolders[3]);
            Assert.AreEqual("compositions/", rootFolders[4]);

            var recordLabel = new RecordLabel();
            rootFolders = StoragePath.RootFolders(recordLabel);
            Assert.AreEqual("images/", rootFolders[0]);
            Assert.AreEqual("pamphlets/", rootFolders[1]);
            Assert.AreEqual("promotional/", rootFolders[2]);
            Assert.AreEqual("contracts/", rootFolders[3]);
            Assert.AreEqual("recordings/", rootFolders[4]);
            Assert.AreEqual("releases/", rootFolders[5]);
            Assert.AreEqual("merchandise/", rootFolders[6]);
            Assert.AreEqual("artists/", rootFolders[7]);
        }

        [TestMethod]
        public void GetContractFoldersTests()
        {
            var publisher = new Publisher();
            var contractFolders = StoragePath.ContractFolders(publisher);
            Assert.IsNotNull(contractFolders);
            Assert.AreEqual("contracts/compositions/", contractFolders[0]);
            Assert.AreEqual("contracts/publications/", contractFolders[1]);
            Assert.AreEqual("contracts/users/", contractFolders[2]);
            Assert.AreEqual("contracts/general/", contractFolders[3]);

            var recordLabel = new RecordLabel();
            contractFolders = StoragePath.ContractFolders(recordLabel);
            Assert.IsNotNull(contractFolders);
            Assert.AreEqual("contracts/recordings/", contractFolders[0]);
            Assert.AreEqual("contracts/releases/", contractFolders[1]);
            Assert.AreEqual("contracts/artists/", contractFolders[2]);
            Assert.AreEqual("contracts/users/", contractFolders[3]);
            Assert.AreEqual("contracts/general/", contractFolders[4]);
        }

        [TestMethod]
        public void GetEntityFolderTests()
        {
            var publication = new Publication { Id = 1 };
            var publicationFolder = StoragePath.GetFolder(publication);
            Assert.AreEqual("publications/publication-1/", publicationFolder);

            var composition = new Composition { Id = 2 };
            var compositionFolder = StoragePath.GetFolder(composition);
            Assert.AreEqual("compositions/composition-2/", compositionFolder);

            var recording = new Recording { Id = 3 };
            var recordingFolder = StoragePath.GetFolder(recording);
            Assert.AreEqual("recordings/recording-3/", recordingFolder);

            var release = new Release { Id = 4 };
            var releaseFolder = StoragePath.GetFolder(release);
            Assert.AreEqual("releases/release-4/", releaseFolder);

            var artist = new Artist { Id = 5 };
            var artistFolder = StoragePath.GetFolder(artist);
            Assert.AreEqual("artists/artist-5/", artistFolder);

            var merchandiseItem = new MerchandiseItem { Id = 6 };
            var merchandiseItemFolder = StoragePath.GetFolder(merchandiseItem);
            Assert.AreEqual("merchandise/merchandise-item-6/", merchandiseItemFolder);

            var merchandiseProduct = new MerchandiseProduct
            {
                Id = 7,
                MerchandiseItem = merchandiseItem
            };
            var merchandiseProductFolder = StoragePath.GetFolder(merchandiseProduct);
            Assert.AreEqual("merchandise/merchandise-item-6/products/product-7/", merchandiseProductFolder);
        }

        [TestMethod]
        public void GetPublisherContractFolderTests()
        {
            var contractTemplate = new PublisherContract { IsTemplate = true };
            var templateFolder = StoragePath.GetFolder(contractTemplate);
            Assert.AreEqual("contracts/templates/", templateFolder);

            var user = new User { Id = 8 };
            var userContract = new PublisherContract();
            var userContractFolder = StoragePath.GetFolder(userContract, user);
            Assert.AreEqual("contracts/users/user-8/", userContractFolder);

            var publicationContract = new PublisherContract { Publication = new Publication { Id = 9 } };
            var publicationContractFolder = StoragePath.GetFolder(publicationContract);
            Assert.AreEqual("contracts/publications/publication-9/", publicationContractFolder);

            var compositionContract = new PublisherContract { Composition = new Composition { Id = 10 } };
            var compositionContractFolder = StoragePath.GetFolder(compositionContract);
            Assert.AreEqual("contracts/compositions/composition-10/", compositionContractFolder);
        }

        [TestMethod]
        public void GetRecordLabelContractFolderTests()
        {
            var contractTemplate = new RecordLabelContract { IsTemplate = true };
            var templateFolder = StoragePath.GetFolder(contractTemplate);
            Assert.AreEqual("contracts/templates/", templateFolder);

            var user = new User { Id = 11 };
            var userContract = new RecordLabelContract();
            var userContractFolder = StoragePath.GetFolder(userContract, user);
            Assert.AreEqual("contracts/users/user-11/", userContractFolder);

            var releaseContract = new RecordLabelContract { Release = new Release { Id = 12 } };
            var releaseContractFolder = StoragePath.GetFolder(releaseContract);
            Assert.AreEqual("contracts/releases/release-12/", releaseContractFolder);

            var recordingContract = new RecordLabelContract { Recording = new Recording { Id = 13 } };
            var recordingContractFolder = StoragePath.GetFolder(recordingContract);
            Assert.AreEqual("contracts/recordings/recording-13/", recordingContractFolder);

            var artistContract = new RecordLabelContract { Artist = new Artist { Id = 14 } };
            var artistContractFolder = StoragePath.GetFolder(artistContract);
            Assert.AreEqual("contracts/artists/artist-14/", artistContractFolder);
        }

        [TestMethod]
        public void GetDocumentFolderTests()
        {
            var document = new Document { DocumentType = DocumentType.Pamphlet };
            var documentFolder = StoragePath.GetFolder(document);
            Assert.AreEqual("pamphlets/", documentFolder);

            document = new Document { DocumentType = DocumentType.Promotional };
            documentFolder = StoragePath.GetFolder(document);
            Assert.AreEqual("promotional/", documentFolder);

            document = new Document { DocumentType = DocumentType.Contract };
            documentFolder = StoragePath.GetFolder(document);
            Assert.AreEqual("contracts/", documentFolder);

            document = new Document { DocumentType = DocumentType.CompositionMaster };
            documentFolder = StoragePath.GetFolder(document);
            Assert.AreEqual("compositions/", documentFolder);

            document = new Document { DocumentType = DocumentType.PublicationMaster };
            documentFolder = StoragePath.GetFolder(document);
            Assert.AreEqual("publications/", documentFolder);

            document = new Document { DocumentType = DocumentType.Metadata };
            documentFolder = StoragePath.GetFolder(document);
            Assert.IsNull(documentFolder);

            document = new Document { DocumentType = DocumentType.Unspecified };
            documentFolder = StoragePath.GetFolder(document);
            Assert.IsNull(documentFolder);
        }
    }
}
