namespace BlueDream
{
    public class PerformanceBillingCredit
    {
        public PerformanceBilling PerformanceBilling { get; set; }

        public Person Person { get; set; }

        public PerformanceRole PerformanceRole { get; set; }

        public string Description { get; set; }

        public bool Hired { get; set; }

        public decimal HiredFor { get; set; }
    }
}