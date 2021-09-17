using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using SongtrackerPro.Data.Enums;

namespace SongtrackerPro.Data.Models
{
    [Table("releases")]
    public class Release
    {
        [Key]
        [Required]
        [Column("id", Order = 1)]
        public int Id { get; set; }

        [Required]
        [Column("title", Order = 2)]
        public string Title { get; set; }

        [JsonIgnore]
        [Required]
        [Column("record_label_id", Order = 3)]
        public int RecordLabelId { get; set; }
        public RecordLabel RecordLabel { get; set; }

        [Required]
        [Column("type", Order = 4)]
        public ReleaseType Type { get; set; }

        [Column("catalog_number", Order = 5)]
        public string CatalogNumber { get; set; }
    }
}
