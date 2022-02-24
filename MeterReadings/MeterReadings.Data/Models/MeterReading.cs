using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeterReadings.Data.Models
{
    public class MeterReading
    {
        [ForeignKey("Account")]
        public int AccountId { get; set; }
        [Required]
        public DateTime MeterReadingDateTime { get; set; }
        [Required]
        public string MeterReadValue { get; set; }

        public Account? Account { get; set; }

        public MeterReading(int accountId, DateTime meterReadingDateTime, string meterReadValue)
        {
            AccountId = accountId;
            MeterReadingDateTime = meterReadingDateTime;
            MeterReadValue = meterReadValue;
        }

        public override bool Equals(object? obj)
        {
            return obj != null && Equals(obj as MeterReading);
        }

        public bool Equals(MeterReading? other)
        {
            return other != null 
                && AccountId == other.AccountId 
                && MeterReadingDateTime == other.MeterReadingDateTime;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(AccountId, MeterReadingDateTime, MeterReadValue);
        }
    }
}
