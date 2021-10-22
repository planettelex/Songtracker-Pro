namespace BlueDream
{
    public class User
    {
        public Person Person { get; set; }

        public UserRoles Roles { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public Image ProfileImage { get; set; }
    }
}