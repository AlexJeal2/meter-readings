using System.ComponentModel.DataAnnotations;

namespace MeterReadings.Models
{
    public class MeterReadingDTO
    {
        [Key]
        [Required]
        public int AccountId { get; set; }
        [Key]
        [Required]
        public DateTime MeterReadingDateTime { get; set; }
        [Required]
        [MaxLength(5)]
        public string MeterReadValue { get; set; }

        public bool IsValid => ValidationErrors.Count == 0;
        public List<string> ValidationErrors { get; set; } = new List<string>();

        public MeterReadingDTO(int accountId, DateTime meterReadingDateTime, string meterReadValue)
        {
            AccountId = accountId;
            MeterReadingDateTime = meterReadingDateTime;
            MeterReadValue = meterReadValue;
        }


        public override bool Equals(object? obj)
        {
            return obj != null && Equals(obj as MeterReadingDTO);
        }

        public bool Equals(MeterReadingDTO? other)
        {
            //Equality for a meter reading is for the same account and time
            return other != null &&
                AccountId.Equals(other.AccountId) &&
                MeterReadingDateTime.Equals(other.MeterReadingDateTime);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(AccountId, MeterReadingDateTime);
        }
    }
}