using System;

namespace BlueDream
{
    public class Image
    {
        public string Path { get; set; }

        public DateTime? TakenOn { get; set; }

        public Person TakenBy { get; set; }
    }
}