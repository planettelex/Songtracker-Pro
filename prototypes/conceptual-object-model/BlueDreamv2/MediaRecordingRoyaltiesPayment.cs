using System;

namespace BlueDream
{
    public class MediaRecordingRoyaltiesPayment
    {
        public decimal Amount { get; set; }

        public DateTime PaidOn { get; set; }

        public MediaRecordingRoyalties MediaRecordingRoyalties { get; set; }
    }
}