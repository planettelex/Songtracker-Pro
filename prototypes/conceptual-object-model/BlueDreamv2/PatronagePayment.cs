using System;

namespace BlueDream
{
    public class PatronagePayment
    {
        public Patronage Patronage { get; set; }

        public decimal Amount { get; set; }

        public DateTime PaidOn { get; set; }

        public double Months { get; set; }
    }
}