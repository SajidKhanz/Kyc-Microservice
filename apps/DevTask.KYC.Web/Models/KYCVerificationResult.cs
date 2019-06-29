using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevTask.KYC.Web.Models
{
    public class KYCVerificationResult
    {
        public long ID { get; set; }
        public Guid TransactionId { get; set; }
        public string PersonData { get; set; }
        public string VerificationResult { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
