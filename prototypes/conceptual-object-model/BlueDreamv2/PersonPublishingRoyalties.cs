namespace BlueDream
{
    public class PersonPublishingRoyalties
    {
        public Person Person { get; set; }

        public FiscalQuarter FiscalQuarter { get; set; }

        public decimal MechanicalRoyaltiesEarned { get; set; }

        public decimal PrintRoyaltiesEarned { get; set; }

        public decimal Total { get; set; }
    }
}