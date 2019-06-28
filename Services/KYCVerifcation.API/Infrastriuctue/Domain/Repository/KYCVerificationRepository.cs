using System;
using DevTask.KYCVerification.Domain.Models;
using DevTask.KYCVerification.Domain.Dbcontexts;


namespace KYCVerifcation.API.Infrastriuctue.Domain.Repository
{
    public class KYCVerificationRepository : IKYCRepository
    {
        private KYCDbContext _dbContext;
        public void AddResult(KYCVerificationResult result)
        {
            _dbContext.KYCVerificationResults.Add(result);
            _dbContext.SaveChanges();
        }

        public KYCVerificationRepository()
        {
            _dbContext = new KYCDbContext();
        }

            public KYCVerificationRepository(KYCDbContext dbContext)
        {
            _dbContext = dbContext;
          
        }


    }
}
