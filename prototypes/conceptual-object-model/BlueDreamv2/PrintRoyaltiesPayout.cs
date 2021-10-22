using System;

namespace BlueDream
{
    public class PrintRoyaltiesPayout
    {
        public PrintRoyalties PrintRoyalties { get; set; }

        public decimal Amount { get; set; }

        public SongAuthor PaidTo { get; set; }

        public DateTime PaidOn { get; set; }
    }
}