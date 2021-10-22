using System;

namespace BlueDream
{
    public class ArtistMerchandiseRoyalties
    {
        public decimal Amount { get; set; }

        public Artist Artist { get; set; }

        public FiscalQuarter FiscalQuarter { get; set; }

        public DateTime? CollectedOn { get; set; }

        public DateTime? PaidOutOn { get; set; }
    }
}