using System;

namespace BlueDream
{
    public class SoftwareSystemVersion
    {
        public SoftwareSystem SoftwareSystem { get; set; }

        public string Version { get; set; }

        public bool IsCurrent { get; set; }

        public DateTime? InstalledOn { get; set; }

        public string RepositoryBranch { get; set; }
    }
}