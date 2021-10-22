using System;

namespace BlueDream
{
    public class MediaRecordingRoyalties
    {
        public decimal Amount { get; set; }

        public decimal DueEachRecording { get; set; }

        public Media Media { get; set; }

        public FiscalQuarter FiscalQuarter { get; set; }

        public DateTime? CollectedOn { get; set; }

        public DateTime? PaidOutOn { get; set; }
    }
}