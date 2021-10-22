using System;

namespace BlueDream
{
    public class MerchandiseItemRoyalties
    {
        public decimal Amount { get; set; }

        public MerchandiseItem MerchandiseItem { get; set; }

        public FiscalQuarter FiscalQuarter { get; set; }

        public DateTime? CollectedOn { get; set; }

        public DateTime? PaidOutOn { get; set; }

        public ArtistMerchandiseRoyalties ArtistMerchandiseRoyalties { get; set; }
    }
}