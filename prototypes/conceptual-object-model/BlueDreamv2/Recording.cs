namespace BlueDream
{
    public class Recording
    {
        public Artist Artist { get; set; }

        public Song Song { get; set; }

        public Subsidiary RecordLabel { get; set; }

        // In Seconds
        public int Length { get; set; }

        public string ISRC { get; set; }

        public bool IsCover { get; set; }
    }
}