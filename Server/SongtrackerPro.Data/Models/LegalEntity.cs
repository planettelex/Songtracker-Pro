using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using SongtrackerPro.Data.Attributes;
using SongtrackerPro.Data.Enums;

namespace SongtrackerPro.Data.Models
{
    [Table("legal_entities")]
    public class LegalEntity
    {
        [Key]
        [Required]
        [Column("id", Order = 1)]
        public int Id { get; set; }

        [Required]
        [Column("type", Order = 2)]
        public LegalEntityType Type { get; set; }

        [Required]
        [Column("name", Order = 3)]
        public string Name { get; set; }

        [Column("trade_name", Order = 4)]
        public string TradeName { get; set; }

        [Encrypted]
        [Column("tax_id", Order = 5)]
        public string TaxId { get; set; }

        [Column("email", Order = 6)]
        public string Email { get; set; }

        [JsonIgnore]
        [Column("address_id", Order = 7)]
        public int? AddressId { get; set; }
        public Address Address { get; set; }

        [JsonIgnore]
        public List<LegalEntityService> LegalEntityServices { get; set; }

        [NotMapped]
        public List<Service> Services
        {
            get
            {
                if (_services == null && LegalEntityServices != null)
                {
                    _services = new List<Service>();
                    _services.AddRange(LegalEntityServices.Select(platformService => platformService.Service));
                }
                return _services;
            }
            set => _services = value;
        }
        private List<Service> _services;
    }
}
