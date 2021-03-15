using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;

namespace SongtrackerPro.Data.Models
{
    [Table("platforms")]
    public class Platform
    {
        [Key]
        [Required]
        [Column("id", Order = 1)]
        public int Id { get; set; }

        [Required]
        [Column("name", Order = 2)]
        public string Name { get; set; }

        [Column("website", Order = 3)]
        public string Website { get; set; }

        [JsonIgnore]
        public List<PlatformService> PlatformServices { get; set; }

        [NotMapped]
        public List<Service> Services
        {
            get
            {
                if (_services == null && PlatformServices != null)
                {
                    _services = new List<Service>();
                    _services.AddRange(PlatformServices.Select(platformService => platformService.Service));
                }
                return _services;
            }
            set => _services = value;
        }
        private List<Service> _services;
    }
}
