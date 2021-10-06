using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using SongtrackerPro.Data.Attributes;

namespace SongtrackerPro.Data.Models
{ 
    public class Publisher : Business
    {
        [JsonIgnore]
        [Column("performing_rights_organization_id", Order = 24)]
        public int? PerformingRightsOrganizationId { get; set; }
        public PerformingRightsOrganization PerformingRightsOrganization { get; set; }

        [Encrypted]
        [Column("performing_rights_organization_publisher_number", Order = 25)]
        public string PerformingRightsOrganizationPublisherNumber { get; set; }
    }
}
