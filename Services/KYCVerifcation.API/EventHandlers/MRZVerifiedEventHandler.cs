using DevTask.EvenBus.DomainEvents;
using DevTask.EvenBus.Events;
using DevTask.KYCVerification.Domain.Dbcontexts;
using DevTask.KYCVerification.Domain.Models;
using KYCVerifcation.API.Infrastriuctue.Domain.Repository;
using KYCVerifcation.API.Models;
using KYCVerifcation.API.Servces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KYCVerifcation.API.EventHandlers
{
    public class MRZVerifiedEventHandler : IKYCEventHandler
    {
   
        IKYCRepository _iKYCRepository;
        IKYCVerifcationService iKYCVerifcation;
        public void Handle(string message)
        {
            MRZVerifiedEvent @event = JsonConvert.DeserializeObject<MRZVerifiedEvent>(message);
            dynamic data = JsonConvert.DeserializeObject(@event.PersonInformationAsJSON);

            KYCVerificationRequest request = GetRequest(data);

            KYCVerifcationServiceResult resultDto = iKYCVerifcation.VerifyPersonInfo(request);
            
            KYCVerificationResult result = new KYCVerificationResult();
            result.VerificationResult = JsonConvert.SerializeObject(resultDto);
            result.TransactionId = @event.TransactionId;
            result.PersonData = @event.PersonInformationAsJSON;
            result.CreatedDate = DateTime.Now;

            _iKYCRepository.AddResult(result);
                       
        }

        private KYCVerificationRequest GetRequest(dynamic data)
        {
            KYCVerificationRequest request = new KYCVerificationRequest()
            {
                AcceptTruliooTermsAndConditions = true,
                CleansedAddress = false,
                ConfigurationName = "Identity Verification",
                ConsentForDataSources = new List<string>() { "Visa Verification" },
                CountryCode = "AU",
                DataFields = new DataFields()
                {
                    PersonInfo = new PersonInfo()
                    {
                        DayOfBirth = 5,
                        FirstGivenName = data.GivenName,
                        FirstSurName = data.LastName,
                      //  MiddleName = "Henry",
                        MinimumAge = 0,
                        MonthOfBirth = 3,
                        YearOfBirth = 1983
                    },
                    Location = new Location()
                    {
                        BuildingNumber = "10",
                        PostalCode = "3108",
                        StateProvinceCode = "VIC",
                        StreetName = "Lawford",
                        StreetType = "St",
                        Suburb = "Doncaster",
                        UnitNumber = "3"
                    },
                    Communication = new Communication() {
                    EmailAddress= "testpersonAU % 40gdctest.com",
                    Telephone= "03 9896 8785"
                },
                    Passport = new Passport
                    {
                        Number = data.PassportNumber
                    }
                }

            };

            return request;


        }

        public MRZVerifiedEventHandler()
        {
            iKYCVerifcation = new TruliooKYCVerifcationService() { Key = "4b27896e1945f6f0c069179552784128", TruilooUrl = "https://gateway.trulioo.com/trial/verifications/v1/verify" };
            _iKYCRepository = new KYCVerificationRepository();
        }


    }
}
