using Newtonsoft.Json;

namespace MeterReadings.Client.Models
{
    public class FailedReading
    {
        public int AccountId { get; set; }
        public DateTime MeterReadingDateTime { get; set; }
        public string MeterReadValue { get; set; }
        public List<string> ValidationErrors { get; set; } = new List<string>();
    }
}