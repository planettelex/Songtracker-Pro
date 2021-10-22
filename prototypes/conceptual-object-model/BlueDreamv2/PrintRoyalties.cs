using System;

namespace BlueDream
{
    public class PrintRoyalties
    {
        public decimal Amount { get; set; }

        public FiscalQuarter FiscalQuarter { get; set; }

        public DateTime? CollectedOn { get; set; }

        public DateTime? PaidOutOn { get; set; }

        public Song Song { get; set; }
    }
}