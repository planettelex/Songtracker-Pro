using System;

namespace BlueDream
{
    public class RecordingRoyaltiesPayment
    {
        public decimal Amount { get; set; }

        public DateTime PaidOn { get; set; }

        public RecordingRoyalties RecordingRoyalties { get; set; }
    }
}