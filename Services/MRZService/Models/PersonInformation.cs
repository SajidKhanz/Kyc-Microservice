using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevTask.MRZService.API.Models
{
    public class PersonInformation
    {
        public string GivenName { get; set; }
        public string LastName { get; set; }
        public string Nationality { get; set; }
        public string BirthDate { get; set; }
    }
}
