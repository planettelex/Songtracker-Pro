namespace BlueDream
{
    public class MediaSales
    {
        public Media Media { get; set; }

        public decimal Revenue { get; set; }

        public FiscalQuarter FiscalQuarter { get; set; }

        public int UnitsSold { get; set; }

        public decimal PublishingRoyaltiesDue { get; set; }

        public MediaPublishingRoyaltiesPayment PublishingRoyaltiesPayment { get; set; }

        public decimal RecordingRoyaltiesDue { get; set; }

        public MediaRecordingRoyaltiesPayment RecordingRoyaltiesPayment { get; set; }
    }
}