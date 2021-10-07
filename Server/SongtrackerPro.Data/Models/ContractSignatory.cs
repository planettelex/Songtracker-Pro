using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace SongtrackerPro.Data.Models
{
    [Table("contract_signatories")]
    public class ContractSignatory
    {
        [Key]
        [Required]
        [Column("id", Order = 1)]
        public int Id { get; set; }

        [JsonIgnore]
        [Required]
        [Column("storage_item_id", Order = 2)]
        public Guid ContractId { get; set; }
        [JsonIgnore]
        public Contract Contract { get; set; }

        [JsonIgnore]
        [Column("person_id", Order = 3)]
        public int? PersonId { get; set; }
        public Person Signatory { get; set; }

        [JsonIgnore]
        [Column("contract_party_id", Order = 4)]
        public int? ContractPartyId { get; set; }
        public ContractParty SignatoryFor { get; set; }

        [Required]
        [Column("signatory_title", Order = 5)]
        public string SignatoryTitle { get; set; }

        [Column("signed_on", Order = 6)]
        public DateTime? SignedOn { get; set; }
    }
}
