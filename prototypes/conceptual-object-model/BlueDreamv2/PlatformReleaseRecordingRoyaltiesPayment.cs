using System;

namespace BlueDream
{
    public class PlatformReleaseRecordingRoyaltiesPayment
    {
        public decimal Amount { get; set; }

        public DateTime PaidOn { get; set; }
        
        public PlatformReleaseRecordingRoyalties PlatformReleaseRecordingRoyalties { get; set; }
    }
}