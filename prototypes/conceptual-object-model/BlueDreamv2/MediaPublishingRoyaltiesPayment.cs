using System;

namespace BlueDream
{
    public class MediaPublishingRoyaltiesPayment
    {
        public decimal Amount { get; set; }

        public DateTime PaidOn { get; set; }

        public MediaPublishingRoyalties MediaPublishingRoyalties { get; set; }
    }
}