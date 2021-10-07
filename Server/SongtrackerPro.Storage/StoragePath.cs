using SongtrackerPro.Data.Enums;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Storage
{
    public static class StoragePath
    {
        public static string[] RootFolders(Publisher publisher)
        {
            if (publisher != null)
                return new[]
                {
                    FolderName.Images + "/",
                    FolderName.Pamphlets + "/",
                    FolderName.Contracts + "/",
                    FolderName.Publications + "/",
                    FolderName.Compositions + "/"
                };

            return null;
        }

        public static string[] ContractFolders(Publisher publisher)
        {
            if (publisher != null)
                return new[]
                {
                    $"{FolderName.Contracts}/{FolderName.Compositions}/",
                    $"{FolderName.Contracts}/{FolderName.Publications}/",
                    $"{FolderName.Contracts}/{FolderName.Users}/",
                    $"{FolderName.Contracts}/{FolderName.General}/"
                };

            return null;
        }

        public static string[] RootFolders(RecordLabel recordLabel)
        {
            if (recordLabel != null)
                return new[]
                {
                    FolderName.Images + "/",
                    FolderName.Pamphlets + "/",
                    FolderName.Promotional + "/",
                    FolderName.Contracts + "/",
                    FolderName.Recordings + "/",
                    FolderName.Releases + "/",
                    FolderName.Merchandise + "/",
                    FolderName.Artists + "/"
                };

            return null;
        }

        public static string[] ContractFolders(RecordLabel recordLabel)
        {
            if (recordLabel != null)
                return  new[]
                {
                    $"{FolderName.Contracts}/{FolderName.Recordings}/",
                    $"{FolderName.Contracts}/{FolderName.Releases}/",
                    $"{FolderName.Contracts}/{FolderName.Artists}/",
                    $"{FolderName.Contracts}/{FolderName.Users}/",
                    $"{FolderName.Contracts}/{FolderName.General}/"
                };

            return null;
        }

        public static string GetFolder(Publication publication) => 
            publication != null ? $"{FolderName.Publications}/{FolderName.Publication(publication.Id)}/" : null;

        public static string GetFolder(Composition composition) => 
            composition != null ? $"{FolderName.Compositions}/{FolderName.Composition(composition.Id)}/" : null;

        public static string GetFolder(Recording recording) => 
            recording != null ? $"{FolderName.Recordings}/{FolderName.Recording(recording.Id)}/" : null;

        public static string GetFolder(Release release) => 
            release != null ? $"{FolderName.Releases}/{FolderName.Release(release.Id)}/" : null;

        public static string GetFolder(Artist artist) => 
            artist != null ? $"{FolderName.Artists}/{FolderName.Artist(artist.Id)}/" : null;

        public static string GetFolder(MerchandiseItem merchandiseItem) => 
            merchandiseItem != null ? $"{FolderName.Merchandise}/{FolderName.MerchandiseItem(merchandiseItem.Id)}/" : null;

        public static string GetFolder(MerchandiseProduct merchandiseProduct)
        {
            if (merchandiseProduct?.MerchandiseItem == null)
                return null;

            var merchandiseItemFolder = GetFolder(merchandiseProduct.MerchandiseItem);
            return $"{merchandiseItemFolder}{FolderName.Products}/{FolderName.Product(merchandiseProduct.Id)}/";
        }

        public static string GetFolder(PublisherContract publisherContract, User user = null)
        {
            if (publisherContract == null)
                return null;

            if (publisherContract.IsTemplate == true)
                return $"{FolderName.Contracts}/{FolderName.Templates}/";

            if (user != null)
                return $"{FolderName.Contracts}/{FolderName.Users}/{FolderName.User(user.Id)}/";

            if (publisherContract.PublicationId.HasValue)
                return $"{FolderName.Contracts}/{FolderName.Publications}/{FolderName.Publication(publisherContract.PublicationId.Value)}/";

            if (publisherContract.Publication != null)
                return $"{FolderName.Contracts}/{FolderName.Publications}/{FolderName.Publication(publisherContract.Publication.Id)}/";

            if (publisherContract.CompositionId.HasValue)
                return $"{FolderName.Contracts}/{FolderName.Compositions}/{FolderName.Composition(publisherContract.CompositionId.Value)}/";

            if (publisherContract.Composition != null)
                return $"{FolderName.Contracts}/{FolderName.Compositions}/{FolderName.Composition(publisherContract.Composition.Id)}/";

            return $"{FolderName.Contracts}/{FolderName.General}/";
        }

        public static string GetFolder(RecordLabelContract recordLabelContract, User user = null)
        {
            if (recordLabelContract == null)
                return null;

            if (recordLabelContract.IsTemplate == true)
                return $"{FolderName.Contracts}/{FolderName.Templates}/";

            if (user != null)
                return $"{FolderName.Contracts}/{FolderName.Users}/{FolderName.User(user.Id)}/";

            if (recordLabelContract.ReleaseId.HasValue)
                return $"{FolderName.Contracts}/{FolderName.Releases}/{FolderName.Release(recordLabelContract.ReleaseId.Value)}/";

            if (recordLabelContract.Release != null)
                return $"{FolderName.Contracts}/{FolderName.Releases}/{FolderName.Release(recordLabelContract.Release.Id)}/";

            if (recordLabelContract.RecordingId.HasValue)
                return $"{FolderName.Contracts}/{FolderName.Recordings}/{FolderName.Recording(recordLabelContract.RecordingId.Value)}/";

            if (recordLabelContract.Recording != null)
                return $"{FolderName.Contracts}/{FolderName.Recordings}/{FolderName.Recording(recordLabelContract.Recording.Id)}/";

            if (recordLabelContract.ArtistId.HasValue)
                return $"{FolderName.Contracts}/{FolderName.Artists}/{FolderName.Artist(recordLabelContract.ArtistId.Value)}/";

            if (recordLabelContract.Artist != null)
                return $"{FolderName.Contracts}/{FolderName.Artists}/{FolderName.Artist(recordLabelContract.Artist.Id)}/";

            return $"{FolderName.Contracts}/{FolderName.General}/";
        }

        public static string GetFolder(Document document)
        {
            if (document == null || document.DocumentType == null)
                return null;

            switch (document.DocumentType)
            {
                case DocumentType.Pamphlet:
                    return FolderName.Pamphlets + "/";
                case DocumentType.Promotional:
                    return FolderName.Promotional + "/";
                case DocumentType.Contract:
                    return FolderName.Contracts + "/";
                case DocumentType.CompositionMaster:
                    return FolderName.Compositions + "/";
                case DocumentType.PublicationMaster:
                    return FolderName.Publications + "/";
            }

            return null;
        }

    }
}