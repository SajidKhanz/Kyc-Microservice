using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevTask.MRZService.API.Config;
using Microsoft.AspNetCore.Mvc;
using DevTask.MRZService.API.Models;
using DevTask.MRZService.API.Services;
using DevTask.EvenBus;
using DevTask.EvenBus.DomainEvents;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;

namespace DevTask.MRZService.API.Controllers
{

    [ApiController]
    public class MRZController : ControllerBase
    {

        IMRZService _service;
        IEventBus _serviceBus;
        private readonly ILogger<MRZController> _logger;

        [Route(Urls.GetMRZData)]
        [HttpPost]
        public ActionResult<string> GetPersonInformationByMRZ(MRZRequest request)
        {
            _logger.LogInformation("Getting MR information from ABBYY service");

            PersonInformation person = _service.GetPersonInformation(request.ImageData);

            if (person != null)
            {
                _logger.LogInformation("Produces event for KYC verification.");
                Guid transactionId = Guid.NewGuid();

                _serviceBus.Publish(new MRZVerifiedEvent() { TransactionId = transactionId, PersonInformationAsJSON = JsonConvert.SerializeObject(person) });
                _logger.LogInformation("Event  published for KYC verification.");
                return Ok(transactionId.ToString());
            }

            return BadRequest();
        }

        public MRZController(IMRZService service,
            IEventBus serviceBus,
            ILogger<MRZController> logger)
        {
            _logger = logger;
            _service = service;
            _serviceBus = serviceBus;
        }





    }
}
