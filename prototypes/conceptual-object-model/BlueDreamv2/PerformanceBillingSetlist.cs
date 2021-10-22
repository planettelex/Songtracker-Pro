namespace BlueDream
{
    public class PerformanceBillingSetlist
    {
        public PerformanceBilling PerformanceBilling { get; set; }

        public int Order { get; set; }

        public Song Song { get; set; }

        public bool IsCover { get; set; }

        public string SongName { get; set; }

        public string OriginalArtist { get; set; }

        public string ISWC { get; set; }
    }
}