using DevTask.Common;
using DevTask.KYC.Web.Config;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DevTask.KYC.Web.Services
{
    public class MRZService : IMRZService
    {
        public string MRZSeriveBaseUrl { get; set; } 
        public string GetPersonInfornmationByMRZ(string imageBase64)
        {
            string transactionId = string.Empty;

            HttpClient _httpClient = new HttpClient(new HttpClientHandler { UseCookies = false });
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, MRZSeriveBaseUrl + ApiURLS.MRZSerivceAPIEndPoints.GetMRZData);
            
            var content = new StringContent(JsonConvert.SerializeObject( new { ImageData = imageBase64 }), Encoding.UTF8, MimeTypes.JSON);
            requestMessage.Content = content;

            var response = _httpClient.SendAsync(requestMessage).Result;

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                transactionId = result;
            }

            return transactionId;
        }
    }
}
