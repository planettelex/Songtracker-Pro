using System;

namespace BlueDream
{
    public class RecordingLicenseRoyalties
    {
        public decimal Amount { get; set; }

        public RecordingLicense RecordingLicense { get; set; }

        public FiscalQuarter FiscalQuarter { get; set; }

        public DateTime? CollectedOn { get; set; }

        public DateTime? PaidOutOn { get; set; }
    }
}