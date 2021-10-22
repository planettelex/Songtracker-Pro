using System;

namespace BlueDream
{
    public class ProductOrder
    {
        public Product Product { get; set; }

        public decimal Cost { get; set; }

        public int Quantity { get; set; }

        public DateTime OrderedOn { get; set; }

        public DateTime? ReceivedOn { get; set; }

        public Vendor OrderedFrom { get; set; }

        public Advance Advance { get; set; }
    }
}