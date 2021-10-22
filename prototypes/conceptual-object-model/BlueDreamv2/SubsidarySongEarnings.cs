namespace BlueDream
{
    public class SubsidarySongEarnings
    {
        public Song Song { get; set; }

        public FiscalQuarter FiscalQuarter { get; set; }

        public Subsidiary Subsidiary { get; set; }

        // Mechanical royalties amount minus total paid out
        public decimal? MechanicalRoyaltiesEarnings { get; set; }

        // Print royalties amount minus total paid out
        public decimal? PrintRoyaltiesEarnings { get; set; }

        // Sum of all other earnings
        public decimal Earnings { get; set; }
    }
}