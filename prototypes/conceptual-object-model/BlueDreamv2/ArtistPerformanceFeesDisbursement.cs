using System;

namespace BlueDream
{
    public class ArtistPerformanceFeesDisbursement
    {
        public PerformanceBilling PerformanceBilling { get; set; }

        public decimal Total { get; set; }

        public decimal ManagementShare { get; set; }

        public decimal TotalMinusManagementShare { get; set; }

        public decimal MembersShare { get; set; }

        public DateTime? PaidOn { get; set; }
    }
}