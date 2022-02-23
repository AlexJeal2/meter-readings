using System.ComponentModel.DataAnnotations;

namespace MeterReadings.Data.Models
{
    public class Account
    {
        [Key]
        [Required]
        public int AccountId { get; set; }
        [Required]
        [MaxLength(64)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(64)]
        public string LastName { get; set; }

        public Account(int accountId, string firstName, string lastName)
        {
            AccountId = accountId;
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
