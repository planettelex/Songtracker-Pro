using System;

namespace BlueDream
{
    public class ArtistRecordingPayment
    {
        public ArtistRecordingRoyalties ForRoyalties { get; set; }

        public decimal Amount { get; set; }

        public DateTime PaidOn { get; set; }
    }
}