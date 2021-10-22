using System;

namespace BlueDream
{
    public class Video
    {
        public string Path { get; set; }

        public DateTime? TakenOn { get; set; }

        public Person TakenBy { get; set; }
    }
}