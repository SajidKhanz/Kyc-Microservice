using KYCVerifcation.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KYCVerifcation.API.Servces
{
    public interface  IKYCVerifcationService
    {
        KYCPersonVerifcationResult VerifyPersonInfo(KYCPersonVerificationRequest request);
    }
}
