using System;

namespace BlueDream
{
    public class RecordingRoyalties
    {
        public Recording Recording { get; set; }

        public RoyaltySource Source { get; set; }

        public FiscalQuarter FiscalQuarter { get; set; }

        public DateTime? CollectedOn { get; set; }

        public DateTime? PaidOutOn { get; set; }

        public MediaRecordingRoyalties FromMedia { get; set; }

        public PlatformReleaseRecordingRoyalties FromPlatformRelease { get; set; }

        public PlatformReleaseTrackRecordingRoyalties FromPlatformReleaseTrack { get; set; }

        public RecordingLicenseRoyalties FromRecordingLicense { get; set; }
    }
}