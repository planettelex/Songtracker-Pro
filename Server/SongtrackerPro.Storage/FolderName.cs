namespace SongtrackerPro.Storage
{
    public static class FolderName
    {
        public static string Artist(int id) => $"artist-{id}";

        public const string Artists = "artists";

        public static string Composition(int id) => $"composition-{id}";

        public const string Compositions = "compositions";

        public const string Contracts = "contracts";

        public const string General = "general";

        public const string Images = "images";

        public static string MerchandiseItem(int id) => $"merchandise-item-{id}";

        public const string Merchandise = "merchandise";

        public const string Pamphlets = "pamphlets";

        public static string Product(int id) => $"product-{id}";

        public const string Products = "products";

        public const string Promotional = "promotional";

        public static string Publisher(int id) => $"publisher-{id}";

        public static string Publication(int id) => $"publication-{id}";

        public const string Publications = "publications";

        public static string RecordLabel(int id) => $"record-label-{id}";

        public static string Recording(int id) => $"recording-{id}";

        public const string Recordings = "recordings";

        public static string Release(int id) => $"release-{id}";

        public const string Releases = "releases";

        public const string Templates = "templates";

        public const string Users = "users";

        public static string User(int id) => $"user-{id}";
    }
}
