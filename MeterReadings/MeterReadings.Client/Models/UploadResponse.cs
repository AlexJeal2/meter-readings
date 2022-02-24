namespace MeterReadings.Client.Models
{
    public class UploadResponse
    {
        public int SuccessCount { get; set; }
        public int FailureCount { get; set; }
        public List<FailedReading> FailedReadings { get; set; }
    }
}
