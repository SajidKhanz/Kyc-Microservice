using DevTask.PersonInformation.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DevTask.PersonInformation.Dbcontexts
{
    public class PersonDbContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=MAI-999247\SQLEXPRESS;Database=PersonDB;Trusted_Connection=True;");
        }


    }
}
