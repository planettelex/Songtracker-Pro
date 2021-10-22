using System;

namespace BlueDream
{
    public class RecordingContract
    {
        public Subsidiary RecordLabel { get; set; }

        public Artist Artist { get; set; }

        public DateTime ExecutedOn { get; set; }

        public DateTime? TerminatedOn { get; set; }

        public decimal RecordingPoints { get; set; }

        public decimal MechandisePoints { get; set; }
    }
}