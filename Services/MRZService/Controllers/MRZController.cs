using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevTask.MRZService.API.Config;
using Microsoft.AspNetCore.Mvc;
using DevTask.MRZService.API.Models;

namespace DevTask.MRZService.API.Controllers
{

    [ApiController]
    public class MRZController : ControllerBase
    {

        [Route(Urls.GetMRZData)]
        [HttpGet]       
        public ActionResult<PersonInformation> GetPersonInformationByMRZ()
        {
            return new PersonInformation();
        }
        

    }
}
