using System;

namespace BlueDream
{
    public class PrintLicensePayment
    {
        public PrintLicense ForLicense { get; set; }

        public decimal Amount { get; set; }

        public Company PaidBy { get; set; }

        public DateTime PaidOn { get; set; }

        public PrintRoyalties PrintRoyalties { get; set; }
    }
}