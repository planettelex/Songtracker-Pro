using System;

namespace BlueDream
{
    public class Patronage
    {
        public User User { get; set; }

        public DateTime ExpiresOn { get; set; }

        public PatronageTerms Terms { get; set; }
    }
}