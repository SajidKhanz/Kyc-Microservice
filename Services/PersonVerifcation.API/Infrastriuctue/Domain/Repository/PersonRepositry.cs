using System;
using DevTask.PersonInformation.Models;
using DevTask.PersonInformation.Dbcontexts;

using Microsoft.EntityFrameworkCore;

namespace DevTask.PersonService.Repository
{
    public class PersonRepository 
    {
        private PersonDbContext _personContext;
        public void AddPerson(Person person)
        {
            _personContext.Persons.Add(person);
            _personContext.SaveChanges();
        }

        public PersonRepository(PersonDbContext personContext)
        {
            _personContext = personContext;
          
        }


    }
}
