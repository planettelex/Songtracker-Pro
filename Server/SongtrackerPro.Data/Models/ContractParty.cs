using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using SongtrackerPro.Data.Enums;

namespace SongtrackerPro.Data.Models
{
    [Table("contract_parties")]
    public class ContractParty
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

        [Required]
        [Column("role", Order = 3)]
        public ContractPartyRole Role { get; set; }

        [Required]
        [Column("is_principal", Order = 4)]
        public bool IsPrincipal { get; set; }

        [JsonIgnore]
        [Column("legal_entity_id", Order = 5)]
        public int? LegalEntityId { get; set; }
        public LegalEntity LegalEntity { get; set; }

        public ICollection<ContractSignatory> Signatories { get; set; }
    }
}
