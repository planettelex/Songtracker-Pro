using System;

namespace BlueDream
{
    public class PersonMerchandiseRoyaltiesPayment
    {
        public Person Person { get; set; }

        public decimal Amount { get; set; }

        public DateTime PaidOn { get; set; }

        public ArtistMerchandiseRoyaltiesDisbursement FromDisbursement { get; set; }
    }
}