using System;

namespace BlueDream
{
    public class MechanicalLicenseProcured
    {
        public Recording Recording { get; set; }

        public MechanicalLicenseType Type { get; set; }

        public MechancialLicenseRate Rate { get; set; }

        public DateTime ProcuredOn { get; set; }

        public Company ProcuredFrom { get; set; }
    }
}