using System;

namespace BlueDream
{
    public class PlatformReleasePublishingRoyaltiesPayment
    {
        public decimal Amount { get; set; }

        public DateTime PaidOn { get; set; }

        public PlatformReleasePublishingRoyalties PlatformReleasePublishingRoyalties { get; set; }
    }
}