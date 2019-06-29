using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevTask.KYC.Web.Config
{
    public class ApiURLS
    {
        public static class MRZSerivceAPIEndPoints
        {
            public const string GetMRZData = "/api/getpersoninformationbymrz/";
        }

        public static class KYCSerivceAPIEndPoints
        {
            public const string GetKYCResult = "/api/getkycresult/{transactionId}/";
        }

    }
}
