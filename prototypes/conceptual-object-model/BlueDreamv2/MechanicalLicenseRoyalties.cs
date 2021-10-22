using System;

namespace BlueDream
{
    public class MechanicalLicenseRoyalties
    {
        public decimal Amount { get; set; }

        public MechanicalLicenseIssued MechanicalLicense { get; set; }

        public FiscalQuarter FiscalQuarter { get; set; }

        public DateTime? CollectedOn { get; set; }

        public DateTime? PaidOutOn { get; set; }
    }
}