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
        public KYCVerificationResponse VerifyPerson(KYCVerificationRequest kycRequest)
        {
            KYCVerificationResponse response = new KYCVerificationResponse();

            try
            {
                KYCVerifcationServiceResult kycResult = _service.VerifyPersonInfo(kycRequest);
                response.Result = kycResult;

            }
            catch(Exception ex)
            {
                response.FailureReason = ex.Message;
            }
                      
            return response;
        }


    }
}
