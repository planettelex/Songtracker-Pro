using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using SongtrackerPro.Data.Attributes;
using SongtrackerPro.Data.Enums;

namespace SongtrackerPro.Data.Models
{
    [Table("users")]
    public class User
    {
        [Key]
        [Required]
        [Column("id", Order = 1)]
        public int Id { get; set; }

        [Required]
        [Column("authentication_id", Order = 2)]
        public string AuthenticationId { get; set; }

        [Required]
        [Column("type", Order = 3)]
        public UserType Type { get; set; }

        [Column("roles", Order = 4)]
        public SystemUserRoles Roles { get; set; }

        [JsonIgnore]
        [Column("person_id", Order = 5)]
        public int? PersonId { get; set; }
        public Person Person { get; set; }

        [Encrypted]
        [Column("social_security_number", Order = 6)]
        public string SocialSecurityNumber { get; set; }

        [JsonIgnore]
        [Column("publisher_id", Order = 7)]
        public int? PublisherId { get; set; }
        public Publisher Publisher { get; set; }

        [JsonIgnore]
        [Column("record_label_id", Order = 8)]
        public int? RecordLabelId { get; set; }
        public RecordLabel RecordLabel { get; set; }

        [JsonIgnore]
        [Column("performing_rights_organization_id", Order = 9)]
        public int? PerformingRightsOrganizationId { get; set; }
        public PerformingRightsOrganization PerformingRightsOrganization { get; set; }

        [Encrypted]
        [Column("performing_rights_organization_member_number", Order = 10)]
        public string PerformingRightsOrganizationMemberNumber { get; set; }

        [Encrypted]
        [Column("sound_exchange_account_number", Order = 11)]
        public string SoundExchangeAccountNumber { get; set; }
    }
}
