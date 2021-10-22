using System;

namespace BlueDream
{
    [Flags]
    public enum UserRoles
    {
        ArtistMember = 0,
        ArtistManager = 1,
        Producer = 2,
        Patron = 4,
        Staff = 8,
        Administrator = 16
    }
}