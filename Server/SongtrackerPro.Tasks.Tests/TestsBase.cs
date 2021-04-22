using SongtrackerPro.Data;
using SongtrackerPro.Utilities;

namespace SongtrackerPro.Tasks.Tests
{
    public abstract class TestsBase
    {
        protected TestsBase()
        {
            TestsModel = new TestsModel(DbContext);
        }

        public ApplicationDbContext DbContext => new ApplicationDbContext(ApplicationSettings.Database.ConnectionString);

        public ApplicationDbContext EmptyDbContext => new ApplicationDbContext(string.Empty);

        public TestsModel TestsModel { get; set; }
    }
}
