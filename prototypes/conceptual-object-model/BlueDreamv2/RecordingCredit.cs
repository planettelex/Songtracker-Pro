namespace BlueDream
{
    public class RecordingCredit
    {
        public Recording Recording { get; set; }

        public RecordingRole Role { get; set; }

        public Person Person { get; set; }

        public string Description { get; set; }

        public bool Hired { get; set; }

        public decimal HiredFor { get; set; }
    }
}