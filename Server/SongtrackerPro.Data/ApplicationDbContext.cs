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
        private static bool IsDiscriminator(IPropertyBase property) => property.Name == "discriminator" || property.PropertyInfo == null;

        public DbSet<Address> Addresses { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<ArtistAccount> ArtistAccounts { get; set; }
        public DbSet<ArtistLink> ArtistLinks { get; set; }
        public DbSet<ArtistManager> ArtistManagers { get; set; }
        public DbSet<ArtistMember> ArtistMembers { get; set; }
        public DbSet<Composition> Compositions { get; set; }
        public DbSet<CompositionAuthor> CompositionAuthors { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<ContractParty> ContractParties { get; set; }
        public DbSet<ContractSignatory> ContractSignatories { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<DigitalMediaUpload> DigitalMediaUploads { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentUpload> DocumentUploads { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Installation> Installation { get; set; }
        public DbSet<LegalEntity> LegalEntities { get; set; }
        public DbSet<LegalEntityClient> LegalEntityClients { get; set; }
        public DbSet<LegalEntityContact> LegalEntityContacts { get; set; }
        public DbSet<LegalEntityService> LegalEntityServices { get; set; }
        public DbSet<Login> Logins { get; set; }
        public DbSet<MerchandiseAsset> MerchandiseAssets { get; set; }
        public DbSet<MerchandiseCategory> MerchandiseCategories { get; set; }
        public DbSet<MerchandiseItem> Merchandise { get; set; }
        public DbSet<MerchandiseProduct> MerchandiseProducts { get; set; }
        public DbSet<Person> People { get; set; }
        public DbSet<PerformingRightsOrganization> PerformingRightsOrganizations { get; set; }
        public DbSet<Platform> Platforms { get; set; }
        public DbSet<PlatformService> PlatformServices { get; set; }
        public DbSet<Publication> Publications { get; set; }
        public DbSet<PublicationAuthor> PublicationAuthors { get; set; }
        public DbSet<PublicationMerchandiseProduct> PublicationProducts { get; set; }
        public DbSet<Publisher> Publishers { get; set; }
        public DbSet<PublisherContract> PublisherContracts { get; set; }
        public DbSet<PublisherMerchandiseItem> PublisherMerchandise { get; set; }
        public DbSet<Recording> Recordings { get; set; }
        public DbSet<RecordingCredit> RecordingCredits { get; set; }
        public DbSet<RecordingCreditRole> RecordingCreditRoles { get; set; }
        public DbSet<RecordingRole> RecordingRoles { get; set; }
        public DbSet<RecordLabel> RecordLabels { get; set; }
        public DbSet<RecordLabelContract> RecordLabelContracts { get; set; }
        public DbSet<RecordLabelMerchandiseItem> RecordLabelMerchandise { get; set; }
        public DbSet<Release> Releases { get; set; }
        public DbSet<ReleaseTrack> ReleaseTracks { get; set; }
        public DbSet<ReleaseMerchandiseProduct> ReleaseProducts { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<StorageItem> StorageItems { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<UserInvitation> UserInvitations { get; set; }
    }
}
