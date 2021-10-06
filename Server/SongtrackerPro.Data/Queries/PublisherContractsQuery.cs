using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Data.Queries
{
    public class PublisherContractsQuery
    {
        public PublisherContractsQuery(Publisher publisher, PublisherContractQueryType queryType = PublisherContractQueryType.General)
        {
            PublisherId = publisher.Id;
            QueryType = queryType;
        }

        public enum PublisherContractQueryType
        {
            Template,
            Client,
            Composition,
            Publication,
            General
        }

        public int PublisherId { get; set; }

        public PublisherContractQueryType QueryType { get; set; }
    }
}
