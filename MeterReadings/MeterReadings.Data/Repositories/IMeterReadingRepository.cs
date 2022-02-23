using MeterReadings.Data.Models;

namespace MeterReadings.Data.Repositories
{
    public interface IMeterReadingRepository
    {
        void AddMeterReadings(Dictionary<int, List<MeterReading>> readingsByAccount);
        void AddMeterReadingsForAccount(int accountId, List<MeterReading> meterReadings);
    }
}