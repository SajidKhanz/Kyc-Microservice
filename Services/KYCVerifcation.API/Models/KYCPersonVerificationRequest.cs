using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KYCVerifcation.API.Models
{
    public class KYCPersonVerificationRequest
    {
        public bool AcceptTruliooTermsAndConditions { get; set; }
        public bool CleansedAddress { get; set; }
        public string ConfigurationName { get; set; }
        public List<string> ConsentForDataSources { get; set; }
        public string CountryCode { get; set; }
        public DataFields DataFields { get; set; }
    }

    public class PersonInfo
    {
        public int DayOfBirth { get; set; }
        public string FirstGivenName { get; set; }
        public string FirstSurName { get; set; }
        public string MiddleName { get; set; }
        public int MinimumAge { get; set; }
        public int MonthOfBirth { get; set; }
        public int YearOfBirth { get; set; }
    }

    public class Location
    {
        public string BuildingNumber { get; set; }
        public string PostalCode { get; set; }
        public string StateProvinceCode { get; set; }
        public string StreetName { get; set; }
        public string StreetType { get; set; }
        public string Suburb { get; set; }
        public string UnitNumber { get; set; }
    }

    public class Communication
    {
        public string EmailAddress { get; set; }
        public string Telephone { get; set; }
    }

    public class Passport
    {
        public string Number { get; set; }
    }

    public class DataFields
    {
        public PersonInfo PersonInfo { get; set; }
        public Location Location { get; set; }
        public Communication Communication { get; set; }
        public Passport Passport { get; set; }
    }

}
