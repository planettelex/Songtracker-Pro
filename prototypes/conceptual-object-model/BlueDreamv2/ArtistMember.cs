using System;

namespace BlueDream
{
    public class ArtistMember
    {
        public Artist Artist { get; set; }

        public Person Member { get; set; }

        public DateTime StartedOn { get; set; }

        public DateTime? EndedOn { get; set; }
    }
}