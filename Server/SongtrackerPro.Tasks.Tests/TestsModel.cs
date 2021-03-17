﻿using System;
using System.Collections.Generic;
using System.Linq;
using SongtrackerPro.Data;
using SongtrackerPro.Data.Models;
using SongtrackerPro.Tasks.GeographicTasks;
using SongtrackerPro.Tasks.InstallationTasks;
using SongtrackerPro.Tasks.PlatformTasks;
using SongtrackerPro.Tasks.PublishingTasks;
using SongtrackerPro.Tasks.RecordLabelTasks;

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
                var usa = countries.SingleOrDefault(c => c.IsoCode.ToUpper() == "USA");
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
                    Country = usa
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
                    Name = nameof(Publisher) + " " + stamp,
                    TaxId = stamp.ToString(),
                    Email = $"test@publisher{stamp}.com",
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
                    var labelIndex = new Random().Next(0, labels.Count);
                    label = labels[labelIndex];
                }

                var stamp = DateTime.Now.Ticks;
                var fiftyFifty = new Random().Next(0, 2) == 0;
                return new Artist
                {
                    Name = nameof(Artist) + " " + stamp,
                    TaxId = stamp.ToString(),
                    HasServiceMark = fiftyFifty,
                    WebsiteUrl = "http://www.artist.com",
                    PressKitUrl = "http://www.presskit.com",
                    RecordLabel = label
                };
            }
        }
    }
}