using System;

namespace BlueDream
{
    public class PlatformReleaseSale
    {
        public PlatformRelease Release { get; set; }

        public decimal Price { get; set; }

        public DateTime SoldOn { get; set; }

        public Customer SoldTo { get; set; }
    }
}