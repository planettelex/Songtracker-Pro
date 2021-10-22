using System;

namespace BlueDream
{
    public class ArtistManager
    {
        public Artist Artist { get; set; }

        public Person Manager { get; set; }

        public DateTime StartedOn { get; set; }

        public DateTime EndedOn { get; set; }

        public double RecordingCommission { get; set; }

        public double MerchandiseCommission { get; set; }
    }
}