using System;

namespace BlueDream
{
    public class MerchandiseItemRoyaltiesPayout
    {
        public MerchandiseItemRoyalties MerchandiseItemRoyalties { get; set; }

        public decimal Amount { get; set; }

        public ArtistMerchandiseRoyalties Payment { get; set; }

        public decimal ArtistShare { get; set; }

        public DateTime? ArtistPaidOn { get; set; }

        public decimal ManagementShare { get; set; }

        public DateTime? ManagementPaidOn { get; set; }
    }
}