using System;

namespace BlueDream
{
    public class ProductRoyalties
    {
        public decimal Amount { get; set; }

        public Product Product { get; set; }

        public FiscalQuarter FiscalQuarter { get; set; }

        public DateTime? CollectedOn { get; set; }

        public DateTime? PaidOutOn { get; set; }

        public MerchandiseItemRoyalties MerchandiseItemRoyalties { get; set; }
    }
}