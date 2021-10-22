namespace BlueDream
{
    public class ArtistRecordingRoyaltiesDisbursement
    {
        public ArtistRecordingRoyalties ArtistRecordingRoyalties { get; set; }

        public decimal TotalAfterRecoupment { get; set; }

        public decimal ManagementShare { get; set; }

        public decimal TotalMinusManagementShare { get; set; }

        public decimal MembersShare { get; set; }
    }
}