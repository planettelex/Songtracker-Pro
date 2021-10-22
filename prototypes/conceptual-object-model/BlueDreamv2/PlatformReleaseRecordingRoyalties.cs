using System;

namespace BlueDream
{
    public class PlatformReleaseRecordingRoyalties
    {
        public decimal Amount { get; set; }

        public decimal DueEachRecording { get; set; }

        public PlatformRelease PlatformRelease { get; set; }

        public FiscalQuarter FiscalQuarter { get; set; }

        public DateTime? CollectedOn { get; set; }

        public DateTime? PaidOutOn { get; set; }
    }
}