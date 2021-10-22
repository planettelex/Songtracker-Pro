namespace BlueDream
{
    public class ArtistRecordingRoyalties
    {
        public decimal Amount { get; set; }

        public Artist Artist { get; set; }

        public FiscalQuarter FiscalQuarter { get; set; }
    }
}