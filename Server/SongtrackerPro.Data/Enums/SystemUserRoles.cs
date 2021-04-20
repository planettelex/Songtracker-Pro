using System;

namespace SongtrackerPro.Data.Enums
{
    [Flags]
    public enum SystemUserRoles
    {
        None = 0,
        Songwriter = 1, // assoc. with composition
        ArtistMember = 2, // assoc. with artist
        ArtistManager = 4, // assoc. with artist
        Producer = 8, // assoc. with recording
        VisualArtist = 16 // assoc. with release or merchandise
    }
}
