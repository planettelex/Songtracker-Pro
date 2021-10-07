using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SongtrackerPro.Data.Models
{
    public class PublisherMerchandiseItem : MerchandiseItem
    {
        [JsonIgnore]
        [Column("publisher_id", Order = 7)]
        public int? PublisherId { get; set; }
        public Publisher Publisher { get; set; }
    }
}
