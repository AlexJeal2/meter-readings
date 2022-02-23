using System.ComponentModel.DataAnnotations;

namespace MeterReadings.Models
{
    public class AccountDTO
    {
        [Key]
        [Required]
        public int AccountId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        public AccountDTO(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
    }
}