using System;

namespace BlueDream
{
    public class PersonPublishingRoyaltiesPayment
    {
        public Person Person { get; set; }
        
        public PersonPublishingRoyalties ForRoyalties { get; set; }

        public decimal Amount { get; set; }

        public DateTime PaidOn { get; set; }
    }
}