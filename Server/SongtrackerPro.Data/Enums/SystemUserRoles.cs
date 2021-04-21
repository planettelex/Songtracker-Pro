using System;

namespace SongtrackerPro.Data.Enums
{
    [Flags]
    public enum SystemUserRoles
    {
        None = 0,
        Songwriter = 1,
        ArtistMember = 2,
        ArtistManager = 4,
        Producer = 8,
        VisualArtist = 16
    }
}
