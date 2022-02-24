using MeterReadings.Models;

namespace MeterReadings.Services
{
    public interface IMeterReadingService
    {
        Task<AddMeterReadingsResultDTO> AddMeterReadingsFromCsvAsync(string csv);
        MeterReadingDTO CreateReadingFromCsvData(string rowData);
        AddMeterReadingsResultDTO GetValidFormatMeterReadingsFromCsv(string csv);
        Dictionary<int, List<MeterReadingDTO>> GroupReadingsByCustomer(AddMeterReadingsResultDTO readings);
        void ValidateReading(MeterReadingDTO reading, AddMeterReadingsResultDTO response);
    }
}