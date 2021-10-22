using System;

namespace BlueDream
{
    public class RecordingRoyaltiesPayout
    {
        public RecordingRoyalties RecordingRoyalties { get; set; }

        public decimal Amount { get; set; }

        public ArtistRecordingRoyalties Payment { get; set; }

        public decimal ArtistShare { get; set; }

        public DateTime? ArtistPaidOn { get; set; }

        public decimal ManagementShare { get; set; }

        public DateTime? ManagementPaidOn { get; set; }
    }
}