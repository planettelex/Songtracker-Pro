using SongtrackerPro.Data;
using SongtrackerPro.Utilities;

namespace SongtrackerPro.Tasks.Tests
{
    public abstract class TestsBase
    {
        public ApplicationDbContext DbContext => new ApplicationDbContext(ApplicationSettings.Database.ConnectionString);

        public ApplicationDbContext EmptyDbContext => new ApplicationDbContext(string.Empty);
    }
}
