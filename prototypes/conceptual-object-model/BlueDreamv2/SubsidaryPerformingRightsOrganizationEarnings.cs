namespace BlueDream
{
    public class SubsidaryPerformingRightsOrganizationEarnings
    {
        public Subsidiary Subsidiary { get; set; }

        public PerformingRightsOrganization PerformingRightsOrganization { get; set; }

        public decimal Amount { get; set; }

        public FiscalQuarter FiscalQuarter { get; set; }
    }
}