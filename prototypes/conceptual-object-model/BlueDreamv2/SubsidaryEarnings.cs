namespace BlueDream
{
    public class SubsidaryEarnings
    {
        public Subsidiary Subsidiary { get; set; }

        public FiscalQuarter FiscalQuarter { get; set; }

        // Mechanical royalties amount minus total paid out
        public decimal? MechanicalRoyaltiesEarnings { get; set; }

        // Print royalties amount minus total paid out
        public decimal? PrintRoyaltiesEarnings { get; set; }

        // Recording royalties amount minus total paid out
        public decimal? RecordingRoyaltiesEarnings { get; set; }

        // Public performance royalties
        public decimal? PublicPerformanceRoyaltiesEarnings { get; set; }

        // Sum of all other earnings
        public decimal Earnings { get; set; }
    }
}