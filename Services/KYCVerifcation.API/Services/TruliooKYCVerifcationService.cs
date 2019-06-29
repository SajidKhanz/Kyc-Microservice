using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using DevTask.Common;
using KYCVerifcation.API.Models;
using Newtonsoft.Json;

namespace KYCVerifcation.API.Servces
{
    public class TruliooKYCVerifcationService : IKYCVerifcationService
    {
        public string Key { get; set; }
        public string TruilooUrl { get; set; }
        public KYCVerifcationServiceResult VerifyPersonInfo(KYCVerificationRequest request)
        {

            KYCVerifcationServiceResult kycResult = null;
            

            HttpClient _httpClient = new HttpClient(new HttpClientHandler { UseCookies = false });
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, TruilooUrl);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MimeTypes.JSON));

            _httpClient.DefaultRequestHeaders.Add("x-trulioo-api-key", Key);
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, MimeTypes.JSON);
            requestMessage.Content = content;
            
            var response = _httpClient.SendAsync(requestMessage).Result;

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                kycResult = JsonConvert.DeserializeObject<KYCVerifcationServiceResult>(result);
            }

            return kycResult;

        }

        
    }
}
