using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevTask.KYCVerification.Domain.Models
{
    public class KYCVerificationResult
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID  {get;set;}
        public Guid TransactionId { get; set; }
        public string PersonData { get; set; }
        public string VerificationResult { get; set; }
        public DateTime CreatedDate { get; set; }
        

    }
}
