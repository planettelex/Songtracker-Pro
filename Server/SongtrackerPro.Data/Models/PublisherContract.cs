using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SongtrackerPro.Data.Models
{
    public class PublisherContract : Contract
    {
        [JsonIgnore]
        [Column("composition_id", Order = 28)]
        public int? CompositionId { get; set; }
        public Composition Composition { get; set; }

        [JsonIgnore]
        [Column("publication_id", Order = 29)]
        public int? PublicationId { get; set; }
        public Publication Publication { get; set; }
    }
}
