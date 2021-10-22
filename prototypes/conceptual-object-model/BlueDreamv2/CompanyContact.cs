namespace BlueDream
{
    public class CompanyContact
    {
        public Company Company { get; set; }

        public Person Contact { get; set; }

        public CompanyRole Role { get; set; }

        public string Title { get; set; }
    }
}