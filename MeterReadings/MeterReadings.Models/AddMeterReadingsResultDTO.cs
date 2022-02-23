namespace MeterReadings.Models
{
    public class AddMeterReadingsResultDTO
    {
        public HashSet<MeterReadingDTO> ValidReadings { get; set; } = new HashSet<MeterReadingDTO>();
        public List<MeterReadingDTO> InvalidReadings { get; set; } = new List<MeterReadingDTO>();
    }
}
