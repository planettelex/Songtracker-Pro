using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using SongtrackerPro.Data.Attributes;
using SongtrackerPro.Data.Enums;

namespace SongtrackerPro.Data.Models
{
    public class User : Person
    {
        [Required]
        [Column("authentication_id", Order = 12)]
        public string AuthenticationId { get; set; }

        [Required]
        [Column("user_type", Order = 13)]
        public UserType UserType { get; set; }

        [Column("roles", Order = 14)]
        public SystemUserRoles Roles { get; set; }

        /* TODO:
        [NotMapped]
        public string Name => Person != null ? Person.FirstAndLastName : GetResource.SystemMessage(ApplicationSettings.Culture, "SUPERUSER");
        */

        [JsonIgnore]
        [Column("publisher_id", Order = 15)]
        public int? PublisherId { get; set; }
        public Publisher Publisher { get; set; }

        [JsonIgnore]
        [Column("record_label_id", Order = 16)]
        public int? RecordLabelId { get; set; }
        public RecordLabel RecordLabel { get; set; }

        [JsonIgnore]
        [Column("performing_rights_organization_id", Order = 17)]
        public int? PerformingRightsOrganizationId { get; set; }
        public PerformingRightsOrganization PerformingRightsOrganization { get; set; }

        [Encrypted]
        [Column("performing_rights_organization_member_number", Order = 18)]
        public string PerformingRightsOrganizationMemberNumber { get; set; }

        [Encrypted]
        [Column("sound_exchange_account_number", Order = 19)]
        public string SoundExchangeAccountNumber { get; set; }
    }
}
