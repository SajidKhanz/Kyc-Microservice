using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DevTask.PersonInformation.Models
{
    public class Person
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
