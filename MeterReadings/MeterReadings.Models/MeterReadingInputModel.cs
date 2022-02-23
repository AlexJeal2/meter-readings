using System.ComponentModel.DataAnnotations;

namespace MeterReadings.Services
{
    public class MeterReadingInputModel
    {
        [Required]
        public int AccountId { get; set; }
        [Required]
        public DateTime MeterReadingDateTime { get; set; }
        [Required]
        [MaxLength(5)]
        public string MeterReadValue { get; set; }

        public bool IsValid => ValidationErrors.Count == 0;
        public List<string> ValidationErrors { get; set; } = new List<string>();

        public MeterReadingInputModel(int accountId, DateTime meterReadingDateTime, string meterReadValue)
        {
            AccountId = accountId;
            MeterReadingDateTime = meterReadingDateTime;
            MeterReadValue = meterReadValue;
        }


        public override bool Equals(object? obj)
        {
            return obj != null && Equals(obj as MeterReadingInputModel);
        }

        public bool Equals(MeterReadingInputModel? other)
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