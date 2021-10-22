using System;

namespace BlueDream
{
    public class ProductRoyaltiesPayment
    {
        public decimal Amount { get; set; }

        public DateTime PaidOn { get; set; }

        public ProductRoyalties ProductRoyalties { get; set; }
    }
}