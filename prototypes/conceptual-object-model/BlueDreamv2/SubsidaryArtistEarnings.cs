namespace BlueDream
{
    public class SubsidaryArtistEarnings
    {
        public Subsidiary Subsidiary { get; set; }

        public Artist Artist { get; set; }

        public FiscalQuarter FiscalQuarter { get; set; }

        // Mechanical royalties amount minus total paid out
        public decimal? MechanicalRoyaltiesEarnings { get; set; }

        // Print royalties amount minus total paid out
        public decimal? PrintRoyaltiesEarnings { get; set; }

        // Recording royalties amount minus total paid out
        public decimal? RecordingRoyaltiesEarnings { get; set; }

        // Sum of all other earnings
        public decimal Earnings { get; set; }
    }
}