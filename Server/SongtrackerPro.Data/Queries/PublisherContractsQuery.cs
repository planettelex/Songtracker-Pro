using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Data.Queries
{
    public class PublisherContractsQuery
    {
        public PublisherContractsQuery(Publisher publisher, PublisherContractType type = PublisherContractType.Template)
        {
            PublisherId = publisher.Id;
        }

        public enum PublisherContractType
        {
            Template,
            Client,
            Composition,
            Publication,
            General
        }

        public int PublisherId { get; set; }

        public PublisherContractType ContractType { get; set; }
    }
}
