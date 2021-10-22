using System;

namespace BlueDream
{
    public class PublicPerformanceRoyalties
    {
        public PerformingRightsOrganization PaidBy { get; set; }

        public decimal Amount { get; set; }

        public DateTime? CollectedOn { get; set; }

        public PerformingRightsOrganizationMembership ForMember { get; set; }
    }
}