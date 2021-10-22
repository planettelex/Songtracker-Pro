using System;

namespace BlueDream
{
    public class PersonPerformanceFeesPayment
    {
        public Person Person { get; set; }

        public decimal Amount { get; set; }

        public DateTime PaidOn { get; set; }

        public ArtistPerformanceFeesDisbursement FromDispersement { get; set; }
    }
}