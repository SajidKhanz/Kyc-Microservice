using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevTask.KYCVerifcation.API.Config;
using DevTask.KYCVerification.Domain.Models;
using KYCVerifcation.API.Infrastriuctue.Domain.Repository;
using KYCVerifcation.API.Models;
using KYCVerifcation.API.Servces;
using Microsoft.AspNetCore.Mvc;

namespace KYCVerifcation.API.Controllers
{

    [ApiController]
    public class KYCController : ControllerBase
    {
        IKYCVerifcationService _service;
        IKYCRepository _kycRepository;

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
            catch (Exception ex)
            {
                response.FailureReason = ex.Message;
            }

            return response;
        }


        [Route(Urls.GetVerificationResult)]
        [HttpGet]
        public ActionResult<KYCVerificationResult> GetVerificationResult([FromRoute]string transactionId)
        {
            _kycRepository = new KYCVerificationRepository();
            KYCVerificationResult result;

            var retries = 0;
            var maxRetries = 5;
            var delay = 5000;
            do
            {

                retries++;
                result = _kycRepository.GetKYCVerificationResult(transactionId);
                if (result != null)
                    break;

                if (retries == maxRetries)
                    return NotFound();

                Task.Delay(delay).Wait();

            } while (true);


            return Ok(result);
        }

    }
}
