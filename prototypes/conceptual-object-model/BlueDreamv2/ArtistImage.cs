namespace BlueDream
{
    public class ArtistImage
    {
        public Artist Artist { get; set; }

        public Image Image { get; set; }

        public int SortOrder { get; set; }

        public bool IsProfile { get; set; }

        public bool IsLogo { get; set; }
    }
}