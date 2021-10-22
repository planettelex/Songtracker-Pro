using System;

namespace BlueDream
{
    public class MediaOrder
    {
        public Media Media { get; set; }

        public decimal Cost { get; set; }

        public decimal Quantity { get; set; }

        public DateTime OrderedOn { get; set; }

        public Vendor OrderedFrom { get; set; }

        public DateTime? ReceivedOn { get; set; }

        public Advance Advance { get; set; }
    }
}