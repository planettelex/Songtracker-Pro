using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SongtrackerPro.Data.Models
{
    public class PublicationMerchandiseProduct : MerchandiseProduct
    {
        [JsonIgnore]
        [Column("publication_id", Order = 10)]
        public int? PublicationId { get; set; }
        [JsonIgnore]
        public Publication Publication { get; set; }

        [Column("issue_number", Order = 11)]
        public string IssueNumber { get; set; }
    }
}
