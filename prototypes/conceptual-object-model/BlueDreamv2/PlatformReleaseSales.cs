namespace BlueDream
{
    public class PlatformReleaseSales
    {
        public PlatformRelease PlatformRelease { get; set; }

        public decimal Revenue { get; set; }

        public FiscalQuarter FiscalQuarter { get; set; }

        public int UnitsSold { get; set; }

        public decimal PublishingRoyaltiesDue { get; set; }


        public decimal RecordingRoyaltiesDue { get; set; }

        public PlatformReleaseRecordingRoyaltiesPayment RecordingRoyaltiesPayment { get; set; }

        public PlatformReleasePublishingRoyaltiesPayment PublishingRoyaltiesPayment { get; set; }
    }
}