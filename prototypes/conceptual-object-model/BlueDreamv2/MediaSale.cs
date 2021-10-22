using System;

namespace BlueDream
{
    public class MediaSale
    {
        public Media Media { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public DateTime SoldOn { get; set; }

        public Customer SoldTo { get; set; }

        public DateTime? ShippedOn { get; set; }
    }
}