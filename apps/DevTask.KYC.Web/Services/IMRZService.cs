using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevTask.KYC.Web.Services
{
    public interface IMRZService
    {
         string GetPersonInfornmationByMRZ(string imageBase64);  

    }
}
