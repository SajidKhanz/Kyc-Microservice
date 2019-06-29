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

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DevTask.KYC.Web.Controllers
{
    [Authorize]
    public class KYCController : Controller
    {

        IMRZService _mRZservice;
        IKYCService _kYCService;

        [Route(Urls.KYCUploadFile)]
        public IActionResult Index()
        {
            return View();
        }

        [Route(Urls.KYCUploadFile)]
        [HttpPost]
        public IActionResult Index(IFormFile file)
        {

            if (file.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    string imageBase64 = Convert.ToBase64String(fileBytes);

                    _mRZservice = new MRZService() { MRZSeriveBaseUrl = "https://localhost:44391" };
                    var transactionId = _mRZservice.GetPersonInfornmationByMRZ(imageBase64);

                    return Redirect(Urls.KYCResult.Replace("{transactionId}", transactionId));

                }
            }

            return View();
        }

        [Route(Urls.KYCResult)]
        [HttpGet]
        public IActionResult KYCResult(string transactionId)
        {
            _kYCService = new KYCService() { KYCSeriveBaseUrl = "https://localhost:44358" };
          var kycResult =  _kYCService.GetKYCResult(transactionId);
            return View(kycResult);

        }

    }
}
