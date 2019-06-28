using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using DevTask.MRZService.API.Models;

namespace DevTask.MRZService.API.Services
{
    public class ABBYYMRZService : IMRZService
    {

        private IWebProxy _proxy;
        private ICredentials _credentials;
        public string ApplicationId { get; set; }
        public string ServiceUrl { get; set; }
        

        /// <summary>
        /// User password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Network credentials
        /// </summary>
        protected ICredentials Credentials
        {
            get
            {
                if (_credentials == null)
                {
                    _credentials = string.IsNullOrEmpty(ApplicationId) || string.IsNullOrEmpty(Password)
                        ? CredentialCache.DefaultNetworkCredentials
                        : new NetworkCredential(ApplicationId, Password);
                }
                return _credentials;
            }
        }

        /// <summary>
        /// Network proxy
        /// </summary>
        protected IWebProxy Proxy
        {
            get
            {
                if (_proxy == null)
                {
                    _proxy = HttpWebRequest.DefaultWebProxy;
                    _proxy.Credentials = CredentialCache.DefaultCredentials;
                }
                return _proxy;
            }
        }
        public PersonInformation GetPersonInformation(string imageBase64)
        {           
            return GetResult(ApplicationId, Password, imageBase64, "English", "xml");

        }


        /// <summary>
        /// Gets file processing result, specified by provided parameters, and returns it as downloadable resource
        /// </summary>
        /// <param name="applicationId">Application id</param>
        /// <param name="password">Password</param>
        /// <param name="fileName">Virtual file path on web server</param>
        /// <param name="language">Recognition language</param>
        /// <param name="exportFormat">Recognition export format</param>
        /// <remarks>Language and export formats specification can be obtained from "https://ocrsdk.com/documentation/apireference/processImage/"</remarks>
        protected PersonInformation GetResult(string applicationId, string password, string imageBase64, string language, string exportFormat)
        {

            PersonInformation person = null;

            // Specifying new post request filling it with file content
            var url = string.Format("{0}/processMRZ",ServiceUrl);

            var request = CreateRequest(url, "POST", Credentials, Proxy);
            FillRequestWithContent(request, imageBase64);

            // Getting task id from response
            var response = GetResponse(request);
            var taskId = GetTaskId(response);

            // Checking if task is completed and downloading result by provided url
            url = string.Format("{0}/getTaskStatus?taskId={1}", ServiceUrl, taskId);
            var resultUrl = string.Empty;
            var status = string.Empty;
            while (status != "Completed")
            {
                System.Threading.Thread.Sleep(1000);
                request = CreateRequest(url, "GET", Credentials, Proxy);
                response = GetResponse(request);
                status = GetStatus(response);
                resultUrl = GetResultUrl(response);
            }

            request = (HttpWebRequest)HttpWebRequest.Create(resultUrl);
            var document = GetResponse(request);

            if (document != null)
            {
                person = MapPerson(document);
            }

            return person;
        }


        public PersonInformation MapPerson(XDocument document)
        {

            PersonInformation personInfo = new PersonInformation();
            IEnumerable<XElement> childList = from el in document.Root.Elements()
                                              select el;

            string mrzType = FindElement(childList, "MrzType")?.ToString();

            personInfo.GivenName =  FindElement(childList, "GivenName");
            personInfo.LastName = FindElement(childList, "LastName");
            personInfo.BirthDate = FindElement(childList, "BirthDate");
            personInfo.Nationality = FindElement(childList, "Nationality");
            personInfo.PassportNumber = FindElement(childList, "DocumentNumber");

            return personInfo;
        }


        public string FindElement(IEnumerable<XElement> elements, string type)
        {
            var element = elements.SingleOrDefault(p => (string)p.Attribute("type") == type);
            string value = element?.Value;

            value = Regex.Replace(value, @"[\r\n]", string.Empty);

            return value?.Trim();

        }

        /// <summary>
        /// Creates new request with defined parameters
        /// </summary>
        protected static HttpWebRequest CreateRequest(string url, string method, ICredentials credentials, IWebProxy proxy)
        {
            var request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.ContentType = "application/octet-stream";
            request.Credentials = credentials;
            request.Method = method;
            request.Proxy = proxy;
            return request;
        }



        /// <summary>
        /// Adds content from local file to request stream
        /// </summary>
        protected static void FillRequestWithContent(HttpWebRequest request, string imageBase64)
        {
            var imageBytes = Convert.FromBase64String(imageBase64);

            request.ContentLength = imageBytes.Length;
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(imageBytes, 0, imageBytes.Length);
            }

        }

        /// <summary>
        /// Gets response xml document
        /// </summary>
        protected static XDocument GetResponse(HttpWebRequest request)
        {
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream stream = response.GetResponseStream())
                {
                    return XDocument.Load(new XmlTextReader(stream));
                }
            }
        }

        /// <summary>
        /// Gets file processing task id from response document
        /// </summary>
        protected static string GetTaskId(XDocument doc)
        {
            var id = string.Empty;
            var task = doc.Root.Element("task");
            if (task != null)
            {
                id = task.Attribute("id").Value;
            }
            return id;
        }

        /// <summary>
        /// Gets task's processing status from response document
        /// </summary>
        protected static string GetStatus(XDocument doc)
        {
            var status = string.Empty;
            var task = doc.Root.Element("task");
            if (task != null)
            {
                status = task.Attribute("status").Value;
            }
            return status;
        }

        /// <summary>
        /// Gets result url to download from response document
        /// </summary>
        /// <remarks> Result url will be available only after task status set to "Complete"</remarks>
        protected static string GetResultUrl(XDocument doc)
        {
            var resultUrl = string.Empty;
            var task = doc.Root.Element("task");
            if (task != null)
            {
                resultUrl = task.Attribute("resultUrl") != null ? task.Attribute("resultUrl").Value : string.Empty;
            }
            return resultUrl;
        }

        /// <summary>
        /// Gets result file extension by export format
        /// </summary>
        protected static string GetExtension(string exportFormat)
        {
            var extension = string.Empty;
            switch (exportFormat.ToLower())
            {
                case "txt":
                    extension = "txt";
                    break;
                case "rtf":
                    extension = "rtf";
                    break;
                case "docx":
                    extension = "docx";
                    break;
                case "xlsx":
                    extension = "xlsx";
                    break;
                case "pptx":
                    extension = "pptx";
                    break;
                case "pdfsearchable":
                case "pdftextandimages":
                    extension = "pdf";
                    break;
                case "xml":
                    extension = "xml";
                    break;
            }
            return extension;
        }

        /// <summary>
        /// Copies input stream to output, returns stream length
        /// </summary>
        private static int copyStream(Stream input, Stream output)
        {
            var buffer = new byte[8 * 1024];
            var streamLength = 0;
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
                streamLength += len;
            }
            return streamLength;
        }
    }
}
