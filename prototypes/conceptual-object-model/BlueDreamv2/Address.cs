namespace BlueDream
{
    public class Address
    {
        public string Street1 { get; set; }

        public string Street2 { get; set; }

        public string City { get; set; }

        public CountryRegion Region { get; set; }

        public Country Country { get; set; }

        public string PostalCode { get; set; }
    }
}