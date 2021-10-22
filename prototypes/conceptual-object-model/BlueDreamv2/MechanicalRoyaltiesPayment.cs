using System;

namespace BlueDream
{
    public class MechanicalRoyaltiesPayment
    {
        public decimal Amount { get; set; }

        public DateTime PaidOn { get; set; }

        public MechanicalRoyalties MechanicalRoyalties { get; set; }
    }
}