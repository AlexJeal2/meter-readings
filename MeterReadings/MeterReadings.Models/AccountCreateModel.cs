using System.ComponentModel.DataAnnotations;

namespace MeterReadings.Models
{
    public class AccountCreateModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        public AccountCreateModel(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
    }
}