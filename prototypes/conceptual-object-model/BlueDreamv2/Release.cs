using System;

namespace BlueDream
{
    public class Release
    {
        public string Name { get; set; }

        public Artist Artist { get; set; }

        public ReleaseType Type { get; set; }

        public int TrackCount { get; set; }

        public bool IsCompilation { get; set; }

        public string UPC { get; set; }

        public DateTime ReleasedOn { get; set; }
    }
}