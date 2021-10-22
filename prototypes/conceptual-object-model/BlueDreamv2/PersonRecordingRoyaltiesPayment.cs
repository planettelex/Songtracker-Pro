using System;

namespace BlueDream
{
    public class PersonRecordingRoyaltiesPayment
    {
        public Person Person { get; set; }

        public decimal Amount { get; set; }

        public DateTime PaidOn { get; set; }

        public ArtistRecordingRoyaltiesDisbursement FromDisbursement { get; set; }
    }
}