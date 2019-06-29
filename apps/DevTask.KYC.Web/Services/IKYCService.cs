using DevTask.KYC.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevTask.KYC.Web.Services
{
    public interface IKYCService
    {
        KYCVerificationResult GetKYCResult(string transactioId);
    }
}
