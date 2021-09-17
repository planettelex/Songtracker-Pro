using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Storage
{
    public class StorageContainer
    {
        public StorageContainer(Publisher publisher)
        {
            if (publisher != null)
                Name = FolderName.Publisher(publisher.Id);
        }

        public StorageContainer(RecordLabel recordLabel)
        {
            if (recordLabel != null)
                Name = FolderName.RecordLabel(recordLabel.Id);
        }

        public string Name { get; }
    }
}
