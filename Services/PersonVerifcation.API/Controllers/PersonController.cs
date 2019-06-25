using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevTask.PersonInformation.Models;
using Microsoft.AspNetCore.Mvc;
using PersonInformation.API.Config;

namespace PersonVerifcation.API.Controllers
{
    
    [ApiController]
    public class PersonController : ControllerBase
    {
        [Route( Urls.GerPersons)]
        [HttpGet]
        public ActionResult<Person> Get()
        {
            return new Person();
        }
    }

       
}
