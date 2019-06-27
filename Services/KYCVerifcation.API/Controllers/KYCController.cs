using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevTask.KYCVerifcation.API.Config;
using KYCVerifcation.API.Models;
using KYCVerifcation.API.Servces;
using Microsoft.AspNetCore.Mvc;

namespace KYCVerifcation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KYCController : ControllerBase
    {
        IKYCVerifcationService _service;

        public KYCController(IKYCVerifcationService service)
        {
            _service = service;
        }

        [Route(Urls.VerifyPerson)]
        [HttpPost]
        public KYCPersonVerifcationResult VerifyPerson(KYCPersonVerificationRequest kycRequest)
        {
            KYCPersonVerifcationResult kycResult = _service.VerifyPersonInfo(kycRequest);
            return kycResult;
        }


    }
}
