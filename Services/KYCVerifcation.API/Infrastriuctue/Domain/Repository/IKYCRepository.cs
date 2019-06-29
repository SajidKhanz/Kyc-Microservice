using DevTask.KYCVerification.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KYCVerifcation.API.Infrastriuctue.Domain.Repository
{
    public interface IKYCRepository
    {
        void AddResult(KYCVerificationResult result);
        KYCVerificationResult GetKYCVerificationResult(string transactionId);
    }
}
