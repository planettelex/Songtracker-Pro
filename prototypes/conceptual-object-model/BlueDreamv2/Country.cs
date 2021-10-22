using System;
using System.Collections.Generic;
using System.Linq;

namespace BlueDream
{
    public class Country
    {
        public string Name { get; set; }

        public string Abbreviation { get; set; }

        public List<CountryRegion> Regions { get; set; }

        public static Country UnitedStates = new Country
        {
            Abbreviation = "USA",
            Name = "United States",
            Regions = new List<CountryRegion>
            {
                new CountryRegion { Abbreviation = "AL", Name = "Alabama" },
                new CountryRegion { Abbreviation = "AK", Name = "Alaska" },
                new CountryRegion { Abbreviation = "AZ", Name = "Arizona" },
                new CountryRegion { Abbreviation = "AR", Name = "Arkansas" },
                new CountryRegion { Abbreviation = "CA", Name = "California" },
                new CountryRegion { Abbreviation = "CO", Name = "Colorado" },
                new CountryRegion { Abbreviation = "CT", Name = "Connecticut" },
                new CountryRegion { Abbreviation = "DE", Name = "Delaware" },
                new CountryRegion { Abbreviation = "DC", Name = "District Of Columbia" },
                new CountryRegion { Abbreviation = "FL", Name = "Florida" },
                new CountryRegion { Abbreviation = "GA", Name = "Georgia" },
                new CountryRegion { Abbreviation = "HI", Name = "Hawaii" },
                new CountryRegion { Abbreviation = "ID", Name = "Idaho" },
                new CountryRegion { Abbreviation = "IL", Name = "Illinois" },
                new CountryRegion { Abbreviation = "IN", Name = "Indiana" },
                new CountryRegion { Abbreviation = "IA", Name = "Iowa" },
                new CountryRegion { Abbreviation = "KS", Name = "Kansas" },
                new CountryRegion { Abbreviation = "KY", Name = "Kentucky" },
                new CountryRegion { Abbreviation = "LA", Name = "Louisiana" },
                new CountryRegion { Abbreviation = "ME", Name = "Maine" },
                new CountryRegion { Abbreviation = "MD", Name = "Maryland" },
                new CountryRegion { Abbreviation = "MA", Name = "Massachusetts" },
                new CountryRegion { Abbreviation = "MI", Name = "Michigan" },
                new CountryRegion { Abbreviation = "MN", Name = "Minnesota" },
                new CountryRegion { Abbreviation = "MS", Name = "Mississippi" },
                new CountryRegion { Abbreviation = "MO", Name = "Missouri" },
                new CountryRegion { Abbreviation = "MT", Name = "Montana" },
                new CountryRegion { Abbreviation = "NE", Name = "Nebraska" },
                new CountryRegion { Abbreviation = "NV", Name = "Nevada" },
                new CountryRegion { Abbreviation = "NH", Name = "New Hampshire" },
                new CountryRegion { Abbreviation = "NJ", Name = "New Jersey" },
                new CountryRegion { Abbreviation = "NM", Name = "New Mexico" },
                new CountryRegion { Abbreviation = "NY", Name = "New York" },
                new CountryRegion { Abbreviation = "NC", Name = "North Carolina" },
                new CountryRegion { Abbreviation = "ND", Name = "North Dakota" },
                new CountryRegion { Abbreviation = "OH", Name = "Ohio" },
                new CountryRegion { Abbreviation = "OK", Name = "Oklahoma" },
                new CountryRegion { Abbreviation = "OR", Name = "Oregon" },
                new CountryRegion { Abbreviation = "PA", Name = "Pennsylvania" },
                new CountryRegion { Abbreviation = "RI", Name = "Rhode Island" },
                new CountryRegion { Abbreviation = "SC", Name = "South Carolina" },
                new CountryRegion { Abbreviation = "SD", Name = "South Dakota" },
                new CountryRegion { Abbreviation = "TN", Name = "Tennessee" },
                new CountryRegion { Abbreviation = "TX", Name = "Texas" },
                new CountryRegion { Abbreviation = "UT", Name = "Utah" },
                new CountryRegion { Abbreviation = "VT", Name = "Vermont" },
                new CountryRegion { Abbreviation = "VA", Name = "Virginia" },
                new CountryRegion { Abbreviation = "WA", Name = "Washington" },
                new CountryRegion { Abbreviation = "WV", Name = "West Virginia" },
                new CountryRegion { Abbreviation = "WI", Name = "Wisconsin" },
                new CountryRegion { Abbreviation = "WY", Name = "Wyoming" }
            }
        };

        public static Country Canada = new Country 
        { 
            Abbreviation = "CAN", 
            Name = "Canada",
            Regions = new List<CountryRegion>
            {
                new CountryRegion { Abbreviation = "AB", Name = "Alberta" },
                new CountryRegion { Abbreviation = "BC", Name = "British Columbia" },
                new CountryRegion { Abbreviation = "MB", Name = "Manitoba" },
                new CountryRegion { Abbreviation = "NB", Name = "New Brunswick" },
                new CountryRegion { Abbreviation = "NL", Name = "Newfoundland and Labrador" },
                new CountryRegion { Abbreviation = "NS", Name = "Nova Scotia" },
                new CountryRegion { Abbreviation = "NT", Name = "Northwest Territories" },
                new CountryRegion { Abbreviation = "NU", Name = "Nunavut" },
                new CountryRegion { Abbreviation = "ON", Name = "Ontario" },
                new CountryRegion { Abbreviation = "PE", Name = "Prince Edward Island" },
                new CountryRegion { Abbreviation = "QC", Name = "Quebec" },
                new CountryRegion { Abbreviation = "SK", Name = "Saskatchewan" },
                new CountryRegion { Abbreviation = "YT", Name = "Yukon" },
            }
        };

        public List<string> RegionAbbreviations()
        {
            return Regions.Select(r => r.Abbreviation).ToList();
        }

        public List<string> RegionNames()
        {
            return Regions.Select(r => r.Name).ToList();
        }

        public CountryRegion GetRegionByAbbreviation(string abbreviation)
        {
            return Regions.FirstOrDefault(r => r.Abbreviation.Equals(abbreviation, StringComparison.InvariantCultureIgnoreCase));
        }

        public CountryRegion GetRegionByName(string name)
        {
            return Regions.FirstOrDefault(r => r.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        public override string ToString()
        {
            return Abbreviation;
        }
    }
}