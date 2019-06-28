using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KYCVerifcation.API.Models
{
    public class KYCVerificationResponse
    {
       public  KYCVerifcationServiceResult Result { get; set; }
        public string FailureReason { get; set; }

        public bool Failed {
            get
            {
                return !string.IsNullOrEmpty(FailureReason);
            }
        }
    }
}
