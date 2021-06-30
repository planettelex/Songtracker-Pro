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
        }
        private readonly ApplicationDbContext _dbContext;

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
                var allServices = listServicesTask.DoTask(null).Data;

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
                var fiftyFifty = new Random().Next(0, 1) == 0;
                return new Artist
                {
                    Name = nameof(Artist) + " " + stamp,
                    TaxId = stamp.ToString(),
                    Email = $"test@artist{stamp}.com",
                    Address = Address,
                    RecordLabel = label,
                    HasServiceMark = fiftyFifty,
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
                    Uuid = Guid.Empty,
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
