using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using SongtrackerPro.Data.Models;

namespace SongtrackerPro.Data
{
    public sealed class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            _connectionString = options.FindExtension<SqlServerOptionsExtension>().ConnectionString;
        }

        public ApplicationDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        private readonly string _connectionString;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }

        public DbSet<Installation> Installation { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<PerformingRightsOrganization> PerformingRightsOrganizations { get; set; }
        public DbSet<Platform> Platforms { get; set; }
        public DbSet<PlatformService> PlatformServices { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<RecordLabel> RecordLabels { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<ArtistMember> ArtistMembers { get; set; }
        public DbSet<ArtistManager> ArtistManagers { get; set; }
        public DbSet<ArtistAccount> ArtistAccounts { get; set; }
        public DbSet<ArtistLink> ArtistLinks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }
    }
}
