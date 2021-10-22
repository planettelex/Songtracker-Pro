namespace BlueDream
{
    public class ArtistMerchandiseRoyaltiesDisbursement
    {
        public ArtistMerchandiseRoyalties ArtistMerchandiseRoyalties { get; set; }

        public decimal TotalAfterRecoupment { get; set; }

        public decimal ManagementRecordingShare { get; set; }

        public decimal ManagementMechandiseShare { get; set; }

        public decimal ManagementShare { get; set; }

        public decimal TotalMinusManagementShare { get; set; }

        public decimal MembersShare { get; set; }
    }
}