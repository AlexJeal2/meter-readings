using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeterReadings.Data.Models
{
    public class Account
    {
        [Key]
        [Required]
        public int AccountId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        public Account(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
