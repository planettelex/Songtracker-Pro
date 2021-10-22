using System;

namespace BlueDream
{
    public class MechanicalRoyalties
    {
        public decimal Amount { get; set; }

        public FiscalQuarter FiscalQuarter { get; set; }

        public RoyaltySource Source { get; set; }

        public Song Song { get; set; }

        public DateTime? CollectedOn { get; set; }

        public DateTime? PaidOutOn { get; set; }

        public decimal PaidOutTotal { get; set; }

        public MediaPublishingRoyalties FromMedia { get; set; }

        public PlatformReleasePublishingRoyalties FromPlatformRelease { get; set; }

        public PlatformReleaseTrackPublishingRoyalties FromPlatformReleaseTrack { get; set; }

        public MechanicalLicenseRoyalties FromMechanicalLicense { get; set; }
    }
}