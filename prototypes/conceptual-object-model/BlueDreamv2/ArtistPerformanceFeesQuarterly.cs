namespace BlueDream
{
    public class ArtistPerformanceFeesQuarterly
    {
        public decimal Fees { get; set; }

        public Artist Artist { get; set; }

        public FiscalQuarter FiscalQuarter { get; set; }
    }
}