using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using SongtrackerPro.Data.Enums;

namespace SongtrackerPro.Data.Models
{
    public class Contract : Document
    {
        public Contract()
        {
            DocumentType = Enums.DocumentType.Contract;
        }

        [Column("is_template", Order = 16)]
        public bool? IsTemplate { get; set; }

        [Column("promisor_party_type", Order = 17)]
        public ContractPartyType? PromisorPartyType { get; set; }

        [Column("promisee_party_type", Order = 18)]
        public ContractPartyType? PromiseePartyType { get; set; }

        [JsonIgnore]
        [Column("provided_by_id", Order = 19)]
        public int? ProvidedById { get; set; }
        public LegalEntity ProvidedBy { get; set; }

        [JsonIgnore]
        [Column("template_id", Order = 20)]
        public Guid? TemplateId { get; set; }
        public Contract Template { get; set; }

        [Column("contract_status", Order = 21)]
        public ContractStatus? ContractStatus { get; set; }

        [Column("drafted_on", Order = 22)]
        public DateTime? DraftedOn { get; set; }

        [Column("provided_on", Order = 23)]
        public DateTime? ProvidedOn { get; set; }

        [Column("proposed_on", Order = 24)]
        public DateTime? ProposedOn { get; set; }

        [Column("rejected_on", Order = 25)]
        public DateTime? RejectedOn { get; set; }

        [Column("executed_on", Order = 26)]
        public DateTime? ExecutedOn { get; set; }

        [Column("expired_on", Order = 27)]
        public DateTime? ExpiredOn { get; set; }
    }
}
