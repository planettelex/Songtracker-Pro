namespace BlueDream
{
    public class SubsidaryRecordingEarnings
    {
        public Subsidiary Subsidiary { get; set; }

        public Recording Recording { get; set; }

        public FiscalQuarter FiscalQuarter { get; set; }

        // Recording royalties amount minus total paid out
        public decimal? RecordingRoyaltiesEarnings { get; set; }

        // Sum of all other earnings
        public decimal Earnings { get; set; }
    }
}