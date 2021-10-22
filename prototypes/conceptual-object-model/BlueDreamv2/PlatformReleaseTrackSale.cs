using System;

namespace BlueDream
{
    public class PlatformReleaseTrackSale
    {
        public PlatformReleaseTrack Track { get; set; }

        public decimal Price { get; set; }

        public DateTime SoldOn { get; set; }

        public Customer SoldTo { get; set; }
    }
}