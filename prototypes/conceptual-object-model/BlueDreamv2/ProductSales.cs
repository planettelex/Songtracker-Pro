namespace BlueDream
{
    public class ProductSales
    {
        public Product Product { get; set; }

        public decimal Revenue { get; set; }

        public FiscalQuarter FiscalQuarter { get; set; }

        public int UnitsSold { get; set; }

        public decimal PrintLicenseFees { get; set; }

        public PrintLicensePayment PrintLicensePayment { get; set; }

        public decimal ProductRoyaltiesDue { get; set; }

        public ProductRoyaltiesPayment ProductRoyaltiesPayment { get; set; }
    }
}