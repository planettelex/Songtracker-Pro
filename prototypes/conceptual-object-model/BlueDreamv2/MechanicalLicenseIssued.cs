using System;

namespace BlueDream
{
    public class MechanicalLicenseIssued
    {
        public Song Song { get; set; }

        public MechanicalLicenseType Type { get; set; }

        public MechancialLicenseRate Rate { get; set; }

        public DateTime IssuedOn { get; set; }

        public Company IssuedTo { get; set; }
    }
}