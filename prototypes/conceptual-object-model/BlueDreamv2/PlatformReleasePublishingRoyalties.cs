using System;

namespace BlueDream
{
    public class PlatformReleasePublishingRoyalties
    {
        public decimal Amount { get; set; }

        public decimal DueEachSong { get; set; }

        public PlatformRelease Release { get; set; }

        public FiscalQuarter FiscalQuarter { get; set; }

        public DateTime? CollectedOn { get; set; }

        public DateTime? PaidOutOn { get; set; }
    }
}