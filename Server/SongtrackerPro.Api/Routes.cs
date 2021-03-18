namespace SongtrackerPro.Api
{
    public static class Routes
    {
        public const string Root = "/";
        public const string Countries = "/countries";
        public const string Services = "/services";
        public const string PerformingRightsOrganizations = "/performing-rights-organizations";
        public const string Platforms = "/platforms";
        public const string Platform = "/platforms/{id}";
        public const string Publishers = "/publishers";
        public const string Publisher = "/publishers/{id}";
        public const string RecordLabels = "/record-labels";
        public const string RecordLabel = "/record-labels/{id}";
        public const string Artists = "/artists";
        public const string Artist = "/artists/{id}";
        public const string ArtistAccounts = "/artists/{artistid}/accounts";
        public const string ArtistAccount = "/artists/{artistid}/accounts/{artistaccountid}";
        public const string ArtistLinks = "/artists/{artistid}/links";
        public const string ArtistLink = "/artists/{artistid}/links/{artistlinkid}";
        public const string Users = "/users";
        public const string User = "/users/{id}";
        public const string UserAccounts = "/users/{userid}/accounts";
        public const string UserAccount = "/users/{userid}/accounts/{useraccountid}";
        public const string Login = "/login";
    }
}
