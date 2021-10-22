using System;

namespace BlueDream
{
    public class PlatformReleaseTrackPublishingRoyalties
    {
        public decimal Amount { get; set; }

        public PlatformReleaseTrack Track { get; set; }

        public FiscalQuarter FiscalQuarter { get; set; }

        public DateTime? CollectedOn { get; set; }

        public DateTime? PaidOutOn { get; set; }
    }
}