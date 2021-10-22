using System;

namespace BlueDream
{
    public class StudioSession
    {
        public Company Studio { get; set; }

        public decimal Cost { get; set; }

        public string Description { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime StopTime { get; set; }

        public Advance Advance { get; set; }
    }
}