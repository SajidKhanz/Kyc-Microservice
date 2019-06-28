using DevTask.KYCVerification.Domain.Models;
using Microsoft.EntityFrameworkCore;


namespace DevTask.KYCVerification.Domain.Dbcontexts
{
    public class KYCDbContext : DbContext
    {
        public DbSet<KYCVerificationResult> KYCVerificationResults { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=MAI-999247\SQLEXPRESS;Database=KYCDB;Trusted_Connection=True;");
        }


    }
}
