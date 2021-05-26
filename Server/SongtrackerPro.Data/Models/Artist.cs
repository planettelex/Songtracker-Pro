using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using SongtrackerPro.Data.Attributes;

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

        [Encrypted]
        [Column("tax_id", Order = 3)]
        public string TaxId { get; set; }

        [Column("email", Order = 4)]
        public string Email { get; set; }

        [JsonIgnore]
        [Column("address_id", Order = 5)]
        public int? AddressId { get; set; }
        public Address Address { get; set; }

        [Required]
        [Column("has_service_mark", Order = 6)]
        public bool HasServiceMark { get; set; }

        [Column("website_url", Order = 7)]
        public string WebsiteUrl { get; set; }

        [Column("press_kit_url", Order = 8)]
        public string PressKitUrl { get; set; }

        [JsonIgnore]
        [Column("record_label_id", Order = 9)]
        public int? RecordLabelId { get; set; }
        public RecordLabel RecordLabel { get; set; }

        public List<ArtistMember> Members { get; set; }

        public List<ArtistManager> Managers { get; set; }

        public List<ArtistAccount> Accounts { get; set; }

        public List<ArtistLink> Links { get; set; }
    }
}
