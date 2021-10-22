using System;

namespace BlueDream
{
    public class MechanicalRoyaltiesPayout
    {
        public MechanicalRoyalties MechanicalRoyalties { get; set; }

        public decimal Amount { get; set; }

        public SongAuthor PaidTo { get; set; }

        public DateTime PaidOn { get; set; }

    }
}