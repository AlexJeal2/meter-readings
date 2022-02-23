using MeterReadings.Data.Repositories;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Text.RegularExpressions;

namespace MeterReadings.Services
{
    public class MeterReadingService : IMeterReadingService
    {
        private readonly IMeterReadingRepository _meterReadingRepository;
        private readonly ILogger<MeterReadingService> _logger;

        public MeterReadingService(IMeterReadingRepository meterReadingRepository, ILogger<MeterReadingService> logger)
        {
            _meterReadingRepository = meterReadingRepository;
            _logger = logger;
        }
        public List<MeterReadingInputModel>? GetMeterReadingsFromCsv(string csv)
        {
            if (string.IsNullOrWhiteSpace(csv)) return null;
            var rows = csv.Split(Environment.NewLine);
            //Still no readings if we don't have at least 2 rows
            if (rows.Length < 2) return null;

            List<MeterReadingInputModel> readings = new List<MeterReadingInputModel>();
            List<MeterReadingInputModel> invalidReadings = new List<MeterReadingInputModel>();

            foreach (string rowData in rows.Skip(1).Where(s => !string.IsNullOrWhiteSpace(s)))
            {
                try
                {
                    var reading = CreateReadingFromCsvData(rowData);

                    if (ReadingIsValid(reading))
                    {
                        readings.Add(reading);
                    }
                    else
                    {
                        invalidReadings.Add(reading);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }
            }
            return readings;
        }

        public MeterReadingInputModel CreateReadingFromCsvData(string rowData)
        {
            var colData = rowData.Split(',');
            var accountId = int.Parse(colData[0]);
            var readTime = DateTime.ParseExact(colData[1], "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            var readValue = colData[2].ToString();

            var reading = new MeterReadingInputModel(accountId, readTime, readValue);
            return reading;
        }

        public bool ReadingIsValid(MeterReadingInputModel reading)
        {
            if (!MeterReadValueIsValid(reading.MeterReadValue))
            {
                reading.ValidationErrors.Add("MeterReadValue must be in the format 'NNNNN'");
            }
            return reading.IsValid;
        }

        private bool MeterReadValueIsValid(string meterReadValue)
        {
            return Regex.IsMatch(meterReadValue, "^\\d{5}$");
        }
    }
}
