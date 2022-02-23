using MeterReadings.Data.Models;

namespace MeterReadings.Data.Repositories
{
    public interface IMeterReadingRepository : IRepositoryBase
    {
        void AddMeterReadings(IEnumerable<MeterReading> meterReadings);
        IQueryable<MeterReading> GetReadingsForAccount(int accountId);
    }
}