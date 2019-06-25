using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
        protected string ApplicationId { get; set; }

        /// <summary>
        /// User password
        /// </summary>
        protected string Password { get; set; }

        /// <summary>
        /// Virtual file path on web server
        /// </summary>
        protected string FilePath { get; set; }

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
            ApplicationId = "My_mrz_reader_test";
            Password = "zu31ZfenDltuZ3tD0ZsQKs6K";
            GetResult(ApplicationId, Password, FilePath, "English", "xml");
            return new PersonInformation();
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
        protected void GetResult(string applicationId, string password, string filePath, string language, string exportFormat)
        {
            // Specifying new post request filling it with file content
            var url = string.Format("http://cloud.ocrsdk.com/processMRZ");
            var localPath = "C:\\Users\\999247\\Documents\\eida1.png";
            var request = CreateRequest(url, "POST", Credentials, Proxy);
            FillRequestWithContent(request, localPath);

            // Getting task id from response
            var response = GetResponse(request);
            var taskId = GetTaskId(response);

            // Checking if task is completed and downloading result by provided url
            url = string.Format("http://cloud.ocrsdk.com/getTaskStatus?taskId={0}", taskId);
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

            using (HttpWebResponse result = (HttpWebResponse)request.GetResponse())
            {
                using (Stream stream = result.GetResponseStream())
                {
                 
                }
            }
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
        protected static void FillRequestWithContent(HttpWebRequest request, string contentPath)
        {
            using (BinaryReader reader = new BinaryReader(File.OpenRead(contentPath)))
            {
                request.ContentLength = reader.BaseStream.Length;
                using (Stream stream = request.GetRequestStream())
                {
                    byte[] buffer = new byte[reader.BaseStream.Length];
                    while (true)
                    {
                        int bytesRead = reader.Read(buffer, 0, buffer.Length);
                        if (bytesRead == 0)
                        {
                            break;
                        }
                        stream.Write(buffer, 0, bytesRead);
                    }
                }
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
