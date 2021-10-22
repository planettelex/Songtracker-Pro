namespace BlueDream
{
    public class PlatformReleaseTrackStreamsQuarterly
    {
        public PlatformReleaseTrack Track { get; set; }

        public decimal Revenue { get; set; }

        public FiscalQuarter FiscalQuarter { get; set; }

        public int Streams { get; set; }

        public decimal MechanicalRoyaltiesDue { get; set; }

        public MechanicalRoyaltiesPayment MechanicalRoyaltiesPayment { get; set; }

        public decimal RecordingRoyaltiesDue { get; set; }

        public RecordingRoyaltiesPayment RecordingRoyaltiesPayment { get; set; }
    }
}