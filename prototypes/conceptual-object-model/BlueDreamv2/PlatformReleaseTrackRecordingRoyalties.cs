using System;

namespace BlueDream
{
    public class PlatformReleaseTrackRecordingRoyalties
    {
        public PlatformReleaseTrack Track { get; set; }

        public decimal Amount { get; set; }

        public FiscalQuarter FiscalQuarter { get; set; }

        public DateTime? CollectedOn { get; set; }

        public DateTime? PaidOutOn { get; set; }
    }
}