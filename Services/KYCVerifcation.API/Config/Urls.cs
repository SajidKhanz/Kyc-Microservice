using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevTask.KYCVerifcation.API.Config
{
    public static class Urls
    {
        public const string VerifyPerson = "~/api/verifyperson/";
        public const string GetVerificationResult = "~/api/getkycresult/{transactionId}";
    }
}
