using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DevTask.Common;
using DevTask.KYC.Web.Config;
using DevTask.KYC.Web.Models;
using Newtonsoft.Json;

namespace DevTask.KYC.Web.Services
{
    public class KYCService : IKYCService
    {

        public string KYCSeriveBaseUrl { get; set; }
        public KYCVerificationResult GetKYCResult(string transactionId)
        {
            KYCVerificationResult kycResult = null;


            HttpClient _httpClient = new HttpClient(new HttpClientHandler { UseCookies = false });
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, KYCSeriveBaseUrl  +  ApiURLS.KYCSerivceAPIEndPoints.GetKYCResult.Replace("{transactionId}", transactionId));
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MimeTypes.JSON));
            

            var response = _httpClient.SendAsync(requestMessage).Result;

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                kycResult = JsonConvert.DeserializeObject<KYCVerificationResult>(result);
            }

            return kycResult;
        }
    }
}
