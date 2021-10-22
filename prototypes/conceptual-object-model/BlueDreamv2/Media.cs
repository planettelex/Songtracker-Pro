namespace BlueDream
{
    public class Media
    {
        public Release Release { get; set; }

        public MediaType MediaType { get; set; }

        public decimal RetailPrice { get; set; }

        public string SKU { get; set; }

        public string UPC { get; set; }

        public int UnitsInStock { get; set; }

        public Image Barcode { get; set; }
    }
}