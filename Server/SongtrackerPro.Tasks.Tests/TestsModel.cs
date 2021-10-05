using System;
using System.Collections.Generic;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Enums;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Data.Services;
using SongtrackerPro.Tasks.ArtistTasks;
using SongtrackerPro.Tasks.GeographicTasks;
using SongtrackerPro.Tasks.InstallationTasks;
using SongtrackerPro.Tasks.MerchandiseTasks;
using SongtrackerPro.Tasks.PersonTasks;
using SongtrackerPro.Tasks.PlatformTasks;
using SongtrackerPro.Tasks.PublishingTasks;
using SongtrackerPro.Tasks.RecordLabelTasks;
using SongtrackerPro.Tasks.UserTasks;

namespace SongtrackerPro.Tasks.Tests
{
    public class TestsModel
    {
        public TestsModel(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;

            var seedInstallation = new SeedInstallation(_dbContext);
            seedInstallation.DoTask(null);

            var seedCountries = new SeedCountries(_dbContext);
            seedCountries.DoTask(null);

            var seedPros = new SeedPerformingRightsOrganizations(_dbContext, new SeedCountries(_dbContext));
            seedPros.DoTask(null);

            var seedServices = new SeedServices(_dbContext);
            seedServices.DoTask(null);

            var seedPlatforms = new SeedPlatforms(_dbContext, new ListServices(_dbContext), new AddPlatform(_dbContext));
            seedPlatforms.DoTask(null);

            var seedGenres = new SeedGenres(_dbContext);
            seedGenres.DoTask(null);

            var seedRecordingRoles = new SeedRecordingRoles(_dbContext);
            seedRecordingRoles.DoTask(null);

            var seedMerchandiseCategories = new SeedMerchandiseCategories(_dbContext);
            seedMerchandiseCategories.DoTask(null);
        }
        private readonly ApplicationDbContext _dbContext;

        public bool FiftyFifty => new Random().Next(0, 1) == 0;

        public Address Address
        {
            get
            {
                var countries = new ListCountries(_dbContext).DoTask(null).Data;
                var us = countries.SingleOrDefault(c => c.IsoCode.ToUpper() == "US");
                var streetNumber = new Random().Next(1000, 10000);
                var postalCode = new Random().Next(10000, 99999);
                var cityIndex = new Random().Next(4);
                var cityAndRegion = _cities[cityIndex];
                var cityAndRegionSplit = cityAndRegion.Split(',');
                var city = cityAndRegionSplit[0];
                var region = cityAndRegionSplit[1];

                return new Address
                {
                    Street = $"{streetNumber} Testing St.",
                    City = city,
                    Region = region,
                    PostalCode = postalCode.ToString(),
                    Country = us
                };
            }
        }
        private readonly List<string> _cities = new List<string> { "Denver,CO", "Los Angeles,CA", "Austin,TX", "Portland,OR", "Seattle,WA" };

        public string PhoneNumber
        {
            get
            {
                var areaCode = new Random().Next(100, 999);
                var first = new Random().Next(100, 999);
                var second = new Random().Next(1000, 9999);
                return $"({areaCode}) {first}-{second}";
            }
        }

        public string SocialSecurityNumber
        {
            get
            {
                var first = new Random().Next(100, 999);
                var second = new Random().Next(100, 999);
                var third = new Random().Next(1000, 9999);
                return $"{first}-{second}-{third}";
            }
        }

        public string AuthenticationToken
        {
            get
            {
                // ReSharper disable StringLiteralTypo
                const string authenticationTokenCharacters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                var tokenChars = new char[15];
                var random = new Random();

                for (var i = 0; i < tokenChars.Length; i++)
                    tokenChars[i] = authenticationTokenCharacters[random.Next(authenticationTokenCharacters.Length)];
                
                return new string(tokenChars);
            }
        }

        public Platform Platform
        {
            get
            {
                var listServicesTask = new ListServices(_dbContext);
                var allServices = listServicesTask.DoTask(ServiceType.Platform).Data;

                const int maxServiceCount = 5;
                var numberOfServices = new Random().Next(1, maxServiceCount);

                var services = new List<Service>();
                for (var i = 0; i < numberOfServices; i++)
                {
                    var randomIndex = new Random().Next(allServices.Count - 1);
                    services.Add(allServices[randomIndex]);
                }

                return new Platform
                {
                    Name = nameof(Data.Models.Platform) + " " + DateTime.Now.Ticks,
                    Website = "https://platform.com",
                    Services = services
                };
            }
        }

        public Genre Genre
        {
            get
            {
                var listGenresTask = new ListGenres(_dbContext);
                var allGenres = listGenresTask.DoTask(null).Data;
                var randomIndex = new Random().Next(allGenres.Count - 1);
                return allGenres[randomIndex];
            }
        }

        public LegalEntity LegalEntity
        {
            get
            {
                var listServicesTask = new ListServices(_dbContext);
                var allServices = listServicesTask.DoTask(ServiceType.LegalEntity).Data;

                const int maxServiceCount = 5;
                var numberOfServices = new Random().Next(1, maxServiceCount);

                var services = new List<Service>();
                for (var i = 0; i < numberOfServices; i++)
                {
                    var randomIndex = new Random().Next(allServices.Count - 1);
                    services.Add(allServices[randomIndex]);
                }

                var stamp = DateTime.Now.Ticks;
                return new LegalEntity
                {
                    Name = nameof(Data.Models.LegalEntity) + " " + stamp,
                    TradeName = nameof(Data.Models.LegalEntity) + " Trade Name",
                    TaxId = stamp.ToString(),
                    Email = $"test@legalentity{stamp}.com",
                    Address = Address,
                    Services = services
                };
            }
        }

        public Publisher Publisher
        {
            get
            {
                var pros = new ListPerformingRightsOrganizations(_dbContext).DoTask(null).Data;
                var ascap = pros.SingleOrDefault(pro => pro.Name.ToUpper() == "ASCAP");

                var stamp = DateTime.Now.Ticks;
                return new Publisher
                {
                    Name = nameof(Publisher) + " " + stamp,
                    TaxId = stamp.ToString(),
                    Email = $"test@publisher{stamp}.com",
                    Phone = PhoneNumber,
                    Address = Address,
                    PerformingRightsOrganizationId = ascap?.Id,
                    PerformingRightsOrganization = ascap,
                    PerformingRightsOrganizationPublisherNumber = new Random().Next(100000, 999999).ToString()
                };
            }
        }

        public Publication Publication(Publisher publisher)
        {
            if (publisher == null)
                return null;

            var stamp = DateTime.Now.Ticks;
            return new Publication
            {
                PublisherId = publisher.Id,
                Publisher = publisher,
                Title = nameof(Publication) + " " + stamp,
                CatalogNumber = "#" + stamp,
                Isbn = "ISBN" + stamp,
                CopyrightedOn = DateTime.Today.AddMonths(-3)
            };
        }

        public Composition Composition(Publisher publisher)
        {
            if (publisher == null)
                return null;

            var stamp = DateTime.Now.Ticks;
            return new Composition
            {
                PublisherId = publisher.Id,
                Publisher = publisher,
                Title = nameof(Composition) + " " + stamp,
                CatalogNumber = "#" + stamp,
                Iswc = "ISWC" + stamp,
                CopyrightedOn = DateTime.Today.AddMonths(-3)
            };
        }

        public RecordLabel RecordLabel
        {
            get
            {
                var stamp = DateTime.Now.Ticks;
                return new RecordLabel
                {
                    Name = nameof(RecordLabel) + " " + stamp,
                    TaxId = stamp.ToString(),
                    Email = $"test@label{stamp}.com",
                    Phone = PhoneNumber,
                    Address = Address
                };
            }
        }

        public Recording Recording(Composition composition, Artist artist, RecordLabel recordLabel)
        {
            if (composition == null || artist == null || recordLabel == null)
                return null;

            var stamp = DateTime.Now.Ticks;
            var genre = Genre;
            return new Recording
            {
                Title = composition.Title,
                Composition = composition,
                CompositionId = composition.Id,
                Artist = artist,
                ArtistId = artist.Id,
                RecordLabel = recordLabel,
                RecordLabelId = recordLabel.Id,
                Genre = genre,
                GenreId = genre.Id,
                Isrc = "ISRC" + stamp,
                SecondsLong = new Random().Next(60, 600)
            };
        }

        public Release Release(Artist artist, RecordLabel recordLabel)
        {
            if (recordLabel == null)
                return null;

            var stamp = DateTime.Now.Ticks;
            var genre = Genre;
            var oneToEight = new Random().Next(1, 8);
            return new Release
            {
                Artist = artist,
                ArtistId = artist?.Id,
                CatalogNumber = "#" + stamp,
                RecordLabel = recordLabel,
                RecordLabelId = recordLabel.Id,
                Genre = genre,
                GenreId = genre.Id,
                Title = nameof(Release) + " " + stamp,
                Type = (ReleaseType)oneToEight
            };
        }

        public ReleaseTrack ReleaseTrack(Release release, Recording recording, int trackNumber)
        {
            return new ReleaseTrack
            {
                Release = release,
                ReleaseId = release.Id,
                Recording = recording,
                RecordingId = recording.Id,
                TrackNumber = trackNumber
            };
        }

        public MerchandiseCategory MerchandiseCategory
        {
            get
            {
                var listCategoriesTask = new ListMerchandiseCategories(_dbContext);
                var allCategories = listCategoriesTask.DoTask(null).Data;
                var randomIndex = new Random().Next(allCategories.Count - 1);
                return allCategories[randomIndex];
            }
        }

        public MerchandiseItem MerchandiseItem(Artist artist)
        {
            var stamp = DateTime.Now.Ticks;
            return new MerchandiseItem
            {
                Artist = artist,
                ArtistId = artist?.Id,
                Category = MerchandiseCategory,
                Name = nameof(MerchandiseItem) + " " + stamp,
                Description = stamp.ToString(),
                IsPromotional = FiftyFifty
            };
        }

        public PublisherMerchandiseItem PublisherMerchandiseItem(Publisher publisher)
        {
            var stamp = DateTime.Now.Ticks;
            return new PublisherMerchandiseItem
            {
                Publisher = publisher,
                PublisherId = publisher.Id,
                Category = MerchandiseCategory,
                Name = nameof(MerchandiseItem) + " " + stamp,
                Description = stamp.ToString(),
                IsPromotional = FiftyFifty
            };
        }

        public RecordLabelMerchandiseItem RecordLabelMerchandiseItem(RecordLabel recordLabel, Artist artist)
        {
            var stamp = DateTime.Now.Ticks;
            return new RecordLabelMerchandiseItem
            {
                Artist = artist,
                ArtistId = artist.Id,
                RecordLabel = recordLabel,
                RecordLabelId = recordLabel.Id,
                Category = MerchandiseCategory,
                Name = nameof(MerchandiseItem) + " " + stamp,
                Description = stamp.ToString(),
                IsPromotional = FiftyFifty
            };
        }

        public MerchandiseProduct MerchandiseProduct(MerchandiseItem merchandiseItem)
        {
            var (colorName, hexValue) = Color;
            var stamp = DateTime.Now.Ticks;
            return new MerchandiseProduct
            {
                MerchandiseItem = merchandiseItem,
                MerchandiseItemId = merchandiseItem.Id,
                Name = nameof(MerchandiseProduct) + " " + stamp,
                ColorName = colorName,
                Color = hexValue,
                Description = stamp.ToString(),
                Size = Size,
                Sku = "Sku " + " " + stamp,
                Upc = "Upc " + " " + stamp
            };
        }

        public PublicationMerchandiseProduct PublicationMerchandiseProduct(Publication publication, MerchandiseItem merchandiseItem)
        {
            var (colorName, hexValue) = Color;
            var stamp = DateTime.Now.Ticks;
            var oneToOneHundred = new Random().Next(1, 100);
            return new PublicationMerchandiseProduct
            {
                Publication = publication,
                PublicationId = publication.Id,
                MerchandiseItem = merchandiseItem,
                MerchandiseItemId = merchandiseItem.Id,
                Name = nameof(PublicationMerchandiseProduct) + " " + stamp,
                ColorName = colorName,
                Color = hexValue,
                Description = stamp.ToString(),
                Size = Size,
                Sku = "Sku " + " " + stamp,
                Upc = "Upc " + " " + stamp,
                IssueNumber = "#" + oneToOneHundred
            };
        }

        public ReleaseMerchandiseProduct ReleaseMerchandiseProduct(Release release, MerchandiseItem merchandiseItem)
        {
            var (colorName, hexValue) = Color;
            var stamp = DateTime.Now.Ticks;
            var oneToTwelve = new Random().Next(1, 12);
            return new ReleaseMerchandiseProduct
            {
                Release = release,
                ReleaseId = release.Id,
                MerchandiseItem = merchandiseItem,
                MerchandiseItemId = merchandiseItem.Id,
                Name = nameof(PublicationMerchandiseProduct) + " " + stamp,
                ColorName = colorName,
                Color = hexValue,
                Description = stamp.ToString(),
                Size = Size,
                Sku = "Sku " + " " + stamp,
                Upc = "Upc " + " " + stamp,
                MediaType = (MediaType)oneToTwelve
            };
        }

        public string Size
        {
            get
            {
                var randomIndex = new Random().Next(Sizes.Length - 1);
                return Sizes[randomIndex];
            }
        }

        public string[] Sizes => new[] { "S", "M", "L" };

        public KeyValuePair<string, string> Color
        {
            get
            {
                var randomIndex = new Random().Next(Colors.Count - 1);
                return Colors.ElementAt(randomIndex);
            }
        }

        public Dictionary<string, string> Colors => new Dictionary<string, string>
        {
            { "Dark Purple", "#1E152A" },
            { "Deep Space Sparkle", "#4E6766" },
            { "Cadet Blue", "#5AB1BB" },
            { "Pistachio", "#A5C882" },
            { "Jasmine", "#F7DD72" }
        };

        public Artist Artist
        {
            get
            {
                var labels = new ListRecordLabels(_dbContext).DoTask(null).Data;
                RecordLabel label = null;

                if (labels != null && labels.Any())
                {
                    var labelIndex = new Random().Next(0, labels.Count - 1);
                    label = labels[labelIndex];
                }

                var stamp = DateTime.Now.Ticks;
                return new Artist
                {
                    Name = nameof(Artist) + " " + stamp,
                    TaxId = stamp.ToString(),
                    Email = $"test@artist{stamp}.com",
                    Address = Address,
                    RecordLabel = label,
                    HasServiceMark = FiftyFifty,
                    WebsiteUrl = "http://www.artist.com",
                    PressKitUrl = "http://www.presskit.com"
                };
            }
        }

        public Person Person
        {
            get
            {
                var firstNameIndex = new Random().Next(0, _firstNames.Count - 1);
                var middleNameIndex = new Random().Next(0, _middleNames.Count - 1);
                var lastNameIndex = new Random().Next(0, _lastNames.Count - 1);
                var nameSuffixIndex = new Random().Next(0, _nameSuffices.Count - 1);
                var stamp = DateTime.Now.Ticks;
                return new Person
                {
                    FirstName = _firstNames[firstNameIndex],
                    MiddleName = _middleNames[middleNameIndex],
                    LastName = _lastNames[lastNameIndex],
                    NameSuffix = _nameSuffices[nameSuffixIndex],
                    Email = $"test@person{stamp}.com",
                    Phone = PhoneNumber,
                    Address = Address
                };
            }
        }
        private readonly List<string> _firstNames = new List<string> { "John", "Scott", "Dave", "Chris", "Robert" };
        private readonly List<string> _middleNames = new List<string> { null, "William", "Q.", "Jefferson", null };
        private readonly List<string> _lastNames = new List<string> { "Smith", "Adams", "Douglas", "Rogers", "Long" };
        private readonly List<string> _nameSuffices = new List<string> { null, "Jr.", "III", null, null };

        public StorageItem StorageItem
        {
            get
            {
                var stamp = DateTime.Now.Ticks;
                var oneToTwenty = new Random().Next(1, 20);
                return new StorageItem
                {
                    Name = nameof(StorageItem) + " " + stamp,
                    Container = "Container " + stamp,
                    FileName = $"filename_{stamp}.pdf",
                    FolderPath = $"folder/{oneToTwenty}/"
                };
            }
        }

        public DigitalMedia DigitalMedia(Artist artist, RecordLabel recordLabel)
        {
            var stamp = DateTime.Now.Ticks;
            var oneToTwenty = new Random().Next(1, 20);
            return new DigitalMedia
            {
                Artist = artist,
                ArtistId = artist?.Id,
                RecordLabel = recordLabel,
                RecordLabelId = recordLabel?.Id,
                Name = nameof(DigitalMedia) + " " + stamp,
                Container = "Container " + stamp,
                FileName = $"filename_{stamp}.pdf",
                FolderPath = $"folder/{oneToTwenty}/",
                IsCompressed = false,
                MediaCategory = DigitalMediaCategory.Audio
            };
        }

        public DigitalMedia DigitalMedia(Publisher publisher)
        {
            var stamp = DateTime.Now.Ticks;
            var oneToTwenty = new Random().Next(1, 20);
            return new DigitalMedia
            {
                Publisher = publisher,
                PublisherId = publisher?.Id,
                Name = nameof(DigitalMedia) + " " + stamp,
                Container = "Container " + stamp,
                FileName = $"filename_{stamp}.pdf",
                FolderPath = $"folder/{oneToTwenty}/",
                IsCompressed = false,
                MediaCategory = DigitalMediaCategory.Image
            };
        }

        public Document Document(Publisher publisher)
        {
            var stamp = DateTime.Now.Ticks;
            var oneToTwenty = new Random().Next(1, 20);
            return new Document
            {
                Publisher = publisher,
                PublisherId = publisher.Id,
                Name = nameof(Document) + " " + stamp,
                Container = "Container " + stamp,
                FileName = $"filename_{stamp}.pdf",
                FolderPath = $"folder/{oneToTwenty}/",
                DocumentType = DocumentType.PublicationMaster
            };
        }

        public Document Document(Artist artist)
        {
            var stamp = DateTime.Now.Ticks;
            var oneToTwenty = new Random().Next(1, 20);
            return new Document
            {
                Artist = artist,
                ArtistId = artist.Id,
                Name = nameof(Document) + " " + stamp,
                Container = "Container " + stamp,
                FileName = $"filename_{stamp}.pdf",
                FolderPath = $"folder/{oneToTwenty}/",
                DocumentType = DocumentType.Promotional
            };
        }

        public Document Document(RecordLabel recordLabel)
        {
            var stamp = DateTime.Now.Ticks;
            var oneToTwenty = new Random().Next(1, 20);
            return new Document
            {
                RecordLabel = recordLabel,
                RecordLabelId = recordLabel.Id,
                Name = nameof(Document) + " " + stamp,
                Container = "Container " + stamp,
                FileName = $"filename_{stamp}.pdf",
                FolderPath = $"folder/{oneToTwenty}/",
                DocumentType = DocumentType.Metadata
            };
        }

        public Contract Contract(Artist artist)
        {
            var stamp = DateTime.Now.Ticks;
            var oneToTwenty = new Random().Next(1, 20);
            return new Contract
            {
                Artist = artist,
                ArtistId = artist.Id,
                Name = nameof(Contract) + " " + stamp,
                Container = "Container " + stamp,
                FileName = $"filename_{stamp}.pdf",
                FolderPath = $"folder/{oneToTwenty}/",
                ContractStatus = ContractStatus.Drafted,
                PromisorPartyType = ContractPartyType.Individual,
                PromiseePartyType = ContractPartyType.Artist,
                DraftedOn = DateTime.UtcNow,
                Parties = new List<ContractParty>()
            };
        }

        public PublisherContract PublisherContract(Publication publication)
        {
            var stamp = DateTime.Now.Ticks;
            var oneToTwenty = new Random().Next(1, 20);
            return new PublisherContract
            {
                Publication = publication,
                PublicationId = publication.Id,
                Publisher = publication.Publisher,
                PublisherId = publication.Publisher.Id,
                Name = nameof(PublisherContract) + " " + stamp,
                Container = "Container " + stamp,
                FileName = $"filename_{stamp}.pdf",
                FolderPath = $"folder/{oneToTwenty}/",
                ContractStatus = ContractStatus.Drafted,
                PromisorPartyType = ContractPartyType.Publisher,
                PromiseePartyType = ContractPartyType.Individual,
                DraftedOn = DateTime.UtcNow.AddDays(-3),
                ProposedOn = DateTime.UtcNow
            };
        }

        public PublisherContract PublisherContract(Publisher publisher)
        {
            var stamp = DateTime.Now.Ticks;
            var oneToTwenty = new Random().Next(1, 20);
            return new PublisherContract
            {
                Publisher = publisher,
                PublisherId = publisher.Id,
                Name = nameof(PublisherContract) + " " + stamp,
                Container = "Container " + stamp,
                FileName = $"filename_{stamp}.pdf",
                FolderPath = $"folder/{oneToTwenty}/",
                ContractStatus = ContractStatus.Drafted,
                PromisorPartyType = ContractPartyType.Publisher,
                PromiseePartyType = ContractPartyType.Individual,
                DraftedOn = DateTime.UtcNow.AddDays(-3),
                ProposedOn = DateTime.UtcNow
            };
        }

        public RecordLabelContract RecordLabelContract(Recording recording, Release release)
        {
            var stamp = DateTime.Now.Ticks;
            var oneToTwenty = new Random().Next(1, 20);
            return new RecordLabelContract
            {
                Release = release,
                ReleaseId = release.Id,
                Recording = recording,
                RecordingId = recording.Id,
                RecordLabel = release.RecordLabel ?? recording.RecordLabel,
                RecordLabelId = release.RecordLabel?.Id ?? recording.RecordLabel?.Id,
                Name = nameof(RecordLabelContract) + " " + stamp,
                Container = "Container " + stamp,
                FileName = $"filename_{stamp}.pdf",
                FolderPath = $"folder/{oneToTwenty}/",
                ContractStatus = ContractStatus.Drafted,
                PromisorPartyType = ContractPartyType.RecordLabel,
                PromiseePartyType = ContractPartyType.Artist,
                DraftedOn = DateTime.UtcNow.AddDays(-3),
                ProposedOn = DateTime.UtcNow.AddDays(-1),
                UpdatedOn = DateTime.UtcNow
            };
        }

        public RecordLabelContract RecordLabelContract(RecordLabel recordLabel)
        {
            var stamp = DateTime.Now.Ticks;
            var oneToTwenty = new Random().Next(1, 20);
            return new RecordLabelContract
            {
                RecordLabel = recordLabel,
                RecordLabelId = recordLabel.Id,
                Name = nameof(RecordLabelContract) + " " + stamp,
                Container = "Container " + stamp,
                FileName = $"filename_{stamp}.pdf",
                FolderPath = $"folder/{oneToTwenty}/",
                ContractStatus = ContractStatus.Drafted,
                PromisorPartyType = ContractPartyType.RecordLabel,
                PromiseePartyType = ContractPartyType.Artist,
                DraftedOn = DateTime.UtcNow.AddDays(-3),
                ProposedOn = DateTime.UtcNow.AddDays(-1),
                UpdatedOn = DateTime.UtcNow
            };
        }

        public User User
        {
            get
            {
                var pros = new ListPerformingRightsOrganizations(_dbContext).DoTask(null).Data;
                var ascap = pros.SingleOrDefault(pro => pro.Name.ToUpper() == "ASCAP");

                var publishers = new ListPublishers(_dbContext).DoTask(null).Data;
                Publisher publisher;
                if (publishers.Any())
                    publisher = publishers[new Random().Next(0, publishers.Count - 1)];
                else
                {
                    publisher = Publisher;
                    new AddPublisher(_dbContext, new FormattingService()).DoTask(publisher);
                }
                
                var recordLabels = new ListRecordLabels(_dbContext).DoTask(null).Data;
                RecordLabel recordLabel;
                if (recordLabels.Any())
                    recordLabel = recordLabels[new Random().Next(0, recordLabels.Count - 1)];
                else
                {
                    recordLabel = RecordLabel;
                    new AddRecordLabel(_dbContext, new FormattingService()).DoTask(recordLabel);
                }
                
                var stamp = DateTime.Now.Ticks;
                var person = Person;
                var email = "test@" + stamp + ".com";
                person.Email = email;
                return new User
                {
                    Type = UserType.SystemAdministrator,
                    AuthenticationId = email,
                    PerformingRightsOrganization = ascap,
                    PerformingRightsOrganizationMemberNumber = new Random().Next(100000, 999999).ToString(),
                    SoundExchangeAccountNumber = new Random().Next(1000000, 9999999).ToString(),
                    Person = person,
                    SocialSecurityNumber = SocialSecurityNumber,
                    Publisher = publisher,
                    RecordLabel = recordLabel
                };
            }
        }

        public UserInvitation UserInvitation
        {
            get
            {
                var publishers = new ListPublishers(_dbContext).DoTask(null).Data;
                Publisher publisher;
                if (publishers.Any())
                    publisher = publishers[new Random().Next(0, publishers.Count - 1)];
                else
                {
                    publisher = Publisher;
                    new AddPublisher(_dbContext, new FormattingService()).DoTask(publisher);
                }
                
                var recordLabels = new ListRecordLabels(_dbContext).DoTask(null).Data;
                RecordLabel recordLabel;
                if (recordLabels.Any())
                    recordLabel = recordLabels[new Random().Next(0, recordLabels.Count - 1)];
                else
                {
                    recordLabel = RecordLabel;
                    new AddRecordLabel(_dbContext, new FormattingService()).DoTask(recordLabel);
                }

                var artists = new ListArtists(_dbContext).DoTask(null).Data;
                Artist artist;
                if (artists.Any())
                    artist = artists[new Random().Next(0, artists.Count - 1)];
                else
                {
                    artist = Artist;
                    new AddArtist(_dbContext, new FormattingService()).DoTask(artist);
                }

                var users = new ListUsers(_dbContext).DoTask(null).Data;

                int invitedByUserId;
                if (users.Any())
                    invitedByUserId = users[new Random().Next(0, users.Count - 1)].Id;
                else
                {
                    var user = User;
                    new AddUser(_dbContext, new AddPerson(_dbContext, new FormattingService()), new FormattingService()).DoTask(user);
                    invitedByUserId = user.Id;
                }

                return new UserInvitation
                {
                    Uuid = Guid.NewGuid(),
                    InvitedByUserId = invitedByUserId,
                    Name = Person.FirstAndLastName,
                    Email = Person.Email,
                    Type = UserType.SystemUser,
                    Roles = SystemUserRoles.Songwriter | SystemUserRoles.ArtistMember,
                    Artist = artist,
                    Publisher = publisher,
                    RecordLabel = recordLabel
                };
            }
        }
    }
}
