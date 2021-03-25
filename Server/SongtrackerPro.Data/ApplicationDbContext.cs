using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.SqlServer.Infrastructure.Internal;
using SongtrackerPro.Data.Attributes;
using SongtrackerPro.Data.Encryption;
using SongtrackerPro.Data.Encryption.Providers;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Utilities;

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
            #if DEBUG
            optionsBuilder.EnableSensitiveDataLogging();
            #endif
            
            optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var encryptionProvider = new AesEncryptionProvider(ApplicationSettings.Database.EncryptionKey);
            var encryptionConverter = new EncryptionConverter(encryptionProvider);
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType != typeof(string) || IsDiscriminator(property)) 
                        continue;

                    var attributes = property.PropertyInfo.GetCustomAttributes(typeof(EncryptedAttribute), false);
                    if (attributes.Any())
                        property.SetValueConverter(encryptionConverter);
                }
            }
        }
        private static bool IsDiscriminator(IPropertyBase property) => property.Name == "Discriminator" || property.PropertyInfo == null;

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
        public DbSet<UserInvitation> UserInvitations { get; set; }
    }
}
