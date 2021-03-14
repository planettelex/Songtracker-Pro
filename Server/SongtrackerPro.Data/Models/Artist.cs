using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SongtrackerPro.Data.Models
{
    [Table("artists")]
    public class Artist
    {
        [Key]
        [Required]
        [Column("id", Order = 1)]
        public int Id { get; set; }

        [Required]
        [Column("name", Order = 2)]
        public string Name { get; set; }

        [Column("tax_id", Order = 3)]
        [MaxLength(20)]
        public string TaxId { get; set; }

        [Required]
        [Column("has_service_mark", Order = 4)]
        public bool HasServiceMark { get; set; }

        [Column("website_url", Order = 5)]
        public string WebsiteUrl { get; set; }

        [Column("press_kit_url", Order = 6)]
        public string PressKitUrl { get; set; }

        [Required]
        [Column("record_label_id", Order = 7)]
        public int RecordLabelId { get; set; }
        public RecordLabel RecordLabel { get; set; }

        public List<ArtistMember> Members { get; set; }

        public List<ArtistManager> Managers { get; set; }

        public List<ArtistAccount> Accounts { get; set; }

        public List<ArtistLink> Links { get; set; }
    }
}
