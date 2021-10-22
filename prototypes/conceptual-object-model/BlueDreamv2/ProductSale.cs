using System;

namespace BlueDream
{
    public class ProductSale
    {
        public Product Product { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public DateTime SoldOn { get; set; }

        public DateTime? ShippedOn { get; set; }

        public Customer SoldTo { get; set; }
    }
}