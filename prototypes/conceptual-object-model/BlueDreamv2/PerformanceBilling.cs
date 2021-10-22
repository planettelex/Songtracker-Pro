using System;

namespace BlueDream
{
    public class PerformanceBilling
    {
        public Performance Performance { get; set; }

        public string ArtistName { get; set; }

        public Artist Artist { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public decimal ArtistFees { get; set; }

        public Video Video { get; set; }
    }
}