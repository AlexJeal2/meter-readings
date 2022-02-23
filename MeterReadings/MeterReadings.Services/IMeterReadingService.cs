
namespace MeterReadings.Services
{
    public interface IMeterReadingService
    {
        List<MeterReadingInputModel> GetMeterReadingsFromCsv(string csv);
    }
}