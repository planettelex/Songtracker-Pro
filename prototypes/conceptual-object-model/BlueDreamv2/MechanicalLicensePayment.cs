using System;

namespace BlueDream
{
    public class MechanicalLicensePayment
    {
        public MechanicalLicenseIssued ForLicense { get; set; }

        public decimal Amount { get; set; }

        public Company PaidBy { get; set; }

        public DateTime PaidOn { get; set; }

        public MechanicalLicenseRoyalties MechanicalLicenseRoyalties { get; set; }
    }
}