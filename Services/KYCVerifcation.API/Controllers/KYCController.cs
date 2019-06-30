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
using Microsoft.Extensions.Logging;

namespace KYCVerifcation.API.Controllers
{

    [ApiController]
    public class KYCController : ControllerBase
    {
        IKYCVerifcationService _service;
        IKYCRepository _kycRepository;


        private readonly ILogger<KYCController> _logger;
        public KYCController(IKYCVerifcationService service,
            ILogger<KYCController> logger)
        {
            _logger = logger;
            _service = service;
        }

        [Route(Urls.VerifyPerson)]
        [HttpPost]
        public KYCVerificationResponse VerifyPerson(KYCVerificationRequest kycRequest)
        {
            KYCVerificationResponse response = new KYCVerificationResponse();

            try
            {
                _logger.LogInformation("Getting KYC information");
                KYCVerifcationServiceResult kycResult = _service.VerifyPersonInfo(kycRequest);
                response.Result = kycResult;

            }
            catch (Exception ex)
            {
                response.FailureReason = "Systrem error!";

            }

            return response;
        }


        [Route(Urls.GetVerificationResult)]
        [HttpGet]
        public ActionResult<KYCVerificationResult> GetVerificationResult([FromRoute]string transactionId)
        {
            _kycRepository = new KYCVerificationRepository();
            _logger.LogInformation("Getting KYC result");

            KYCVerificationResult result;

            var retries = 0;
            var maxRetries = 5;
            var delay = 5000;
            do
            {

                retries++;
                result = _kycRepository.GetKYCVerificationResult(transactionId);
                if (result != null || retries == maxRetries)
                    break;

               
                Task.Delay(delay).Wait();

            } while (true);


            return Ok(result);
        }

    }
}
