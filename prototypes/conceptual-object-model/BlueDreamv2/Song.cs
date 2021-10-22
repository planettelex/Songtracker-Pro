using System;

namespace BlueDream
{
    public class Song
    {
        public string Name { get; set; }

        public Artist Artist { get; set; }

        public PublishingSplitMethod SplitMethod { get; set; }

        public string ISWC { get; set; }

        public Subsidiary Publisher { get; set; }

        public string Lyrics { get; set; }

        public bool HasExplicitLyrics { get; set; }

        public DateTime PublishedOn { get; set; }
    }
}