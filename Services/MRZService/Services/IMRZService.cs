using DevTask.MRZService.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevTask.MRZService.API.Services
{
    public interface IMRZService
    {
         PersonInformation GetPersonInformation(string imageBase64);


    }
}
