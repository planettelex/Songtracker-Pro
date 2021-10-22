using System;

namespace BlueDream
{
    public class RecordingLicensePayment
    {
        public RecordingLicense ForLicense { get; set; }

        public decimal Amount { get; set; }

        public DateTime PaidOn { get; set; }

        public Company PaidBy { get; set; }

        public decimal PublishingRoyaltiesDue { get; set; }

        public MechanicalRoyaltiesPayment PublishingRoyaltiesPayment { get; set; }

        public decimal RecordingRoyaltiesDue { get; set; }

        public RecordingRoyaltiesPayment RecordingRoyaltiesPayment { get; set; }
    }
}