using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DevTask.KYC.Web.Config;
using DevTask.KYC.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DevTask.KYC.Web.Controllers
{
    [Authorize]
    public class KYCController : Controller
    {

        IMRZService _mRZservice;
        IKYCService _kYCService;

        private readonly ILogger<KYCController> _logger;
        
        public KYCController(IMRZService mRZservice,
        IKYCService kYCService,
        ILogger<KYCController> logger
        )
        {
            _mRZservice = mRZservice;
            _kYCService = kYCService;
            _logger = logger;
        }

        [Route(Urls.KYCUploadFile)]
        public IActionResult Index()
        {
            return View();
        }

        [Route(Urls.KYCUploadFile)]
        [HttpPost]
        public IActionResult Index(IFormFile file)
        {

            _logger.LogInformation("File upload started");

            if (file.Length > 0)
            {
                _logger.LogInformation("File upload size is " + file.Length);

                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    string imageBase64 = Convert.ToBase64String(fileBytes);
                                        
                    var transactionId = _mRZservice.GetPersonInfornmationByMRZ(imageBase64);

                    _logger.LogInformation("File uploaded and transaction id " + transactionId);
                    return Redirect(Urls.KYCResult.Replace("{transactionId}", transactionId));

                }
            }

            return View();
        }

        [Route(Urls.KYCResult)]
        [HttpGet]
        public IActionResult KYCResult(string transactionId)
        {
            
          var kycResult =  _kYCService.GetKYCResult(transactionId);
            return View(kycResult);

        }

    }
}
