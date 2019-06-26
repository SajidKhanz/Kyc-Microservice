using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevTask.MRZService.API.Config;
using Microsoft.AspNetCore.Mvc;
using DevTask.MRZService.API.Models;
using DevTask.MRZService.API.Services;

namespace DevTask.MRZService.API.Controllers
{

    [ApiController]
    public class MRZController : ControllerBase
    {
        IMRZService _service;

        [Route(Urls.GetMRZData)]
        [HttpGet]       
        public ActionResult<PersonInformation> GetPersonInformationByMRZ()
        {
            
           return _service.GetPersonInformation(""); 
        }

        public MRZController(IMRZService service)
        {
            _service = service;
        }
        

    }
}
