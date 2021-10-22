namespace BlueDream
{
    public class CountryRegion
    {
        public string Name { get; set; }

        public string Abbreviation { get; set; }

        public override string ToString()
        {
            return Abbreviation;
        }
    }
}