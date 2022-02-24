using AutoMapper;
using MeterReadings.Data.Models;
using MeterReadings.Data.Repositories;
using MeterReadings.Models;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

[assembly: InternalsVisibleTo("MeterReadings.Tests")]
namespace MeterReadings.Services
{
    public class MeterReadingService : IMeterReadingService
    {
        private readonly IMeterReadingRepository _meterReadingRepository;
        private readonly ILogger<MeterReadingService> _logger;
        private readonly IMapper _mapper;
        private readonly IAccountRepository _accountRepository;

        public MeterReadingService(
            IMeterReadingRepository meterReadingRepository, 
            IAccountRepository accountRepository, 
            IMapper mapper, ILogger<MeterReadingService> logger)
        {
            _meterReadingRepository = meterReadingRepository;
            _accountRepository = accountRepository;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<AddMeterReadingsResultDTO> AddMeterReadingsFromCsvAsync(string csv)
        {
            var result = GetValidFormatMeterReadingsFromCsv(csv);
            var groupedResults = GroupReadingsByCustomer(result);

            var missingAccountIds = GetMissingAccountIds(groupedResults.Keys);

            foreach (var (accountId, readings) in groupedResults)
            {
                if (!ValidateAccountExists(missingAccountIds, accountId,readings, result))
                {
                    continue;
                }

                ValidateNoLaterRecordExists(accountId, readings, result);
            }

            var newReadings = _mapper.Map<IEnumerable<MeterReading>>(result.ValidReadings);
            _meterReadingRepository.AddMeterReadings(newReadings);
            await _meterReadingRepository.SaveChangesAsync();
            return result;
        }

        private void ValidateNoLaterRecordExists(int accountId, List<MeterReadingDTO> readings, AddMeterReadingsResultDTO result)
        {
            var latestAccountReading = _meterReadingRepository.GetReadingsForAccount(accountId)
                    .OrderByDescending(x => x.MeterReadingDateTime)
                    .FirstOrDefault();

            if (latestAccountReading == null)
            {
                return;
            }

            var invalidReadings = readings.Where(r => r.MeterReadingDateTime <= latestAccountReading.MeterReadingDateTime);
            foreach (var reading in invalidReadings)
            {
                reading.ValidationErrors.Add($"A later reading already exists for AccountId: {accountId}  ({latestAccountReading.MeterReadingDateTime})");
                result.ValidReadings.Remove(reading);
                result.InvalidReadings.Add(reading);
            }
        }

        public AddMeterReadingsResultDTO GetValidFormatMeterReadingsFromCsv(string csv)
        {
            var result = new AddMeterReadingsResultDTO();
            if (string.IsNullOrWhiteSpace(csv)) return result;

            var rows = csv.Split(Environment.NewLine).Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();
            //Still no readings if we don't have at least 2 rows
            if (rows.Length < 2) return result;

            foreach (string rowData in rows.Skip(1))
            {
                try
                {
                    var reading = CreateReadingFromCsvData(rowData);
                    ValidateReading(reading, result);

                    if (!reading.IsValid)
                    {
                        //Remove it from valid just in case we had a duplicate
                        result.ValidReadings.Remove(reading);
                        result.InvalidReadings.Add(reading);
                    }
                    else
                    {
                        result.ValidReadings.Add(reading);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }
            }

            return result;
        }

        public Dictionary<int, List<MeterReadingDTO>> GroupReadingsByCustomer(AddMeterReadingsResultDTO readings)
        {
            //Group account readings by Account ID
            //Sort them in reading date order before inserting
            var accountReadings = readings.ValidReadings
                .GroupBy(r => r.AccountId)
                .ToDictionary(group => group.Key, readings => readings.OrderBy(reading => reading.MeterReadingDateTime).ToList());

            return accountReadings;
        }

        public MeterReadingDTO CreateReadingFromCsvData(string rowData)
        {
            var colData = rowData.Split(',');
            var accountId = int.Parse(colData[0]);
            var readTime = DateTime.ParseExact(colData[1], "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            var readValue = colData[2].ToString();

            var reading = new MeterReadingDTO(accountId, readTime, readValue);
            return reading;
        }

        public void ValidateReading(MeterReadingDTO reading, AddMeterReadingsResultDTO response)
        {
            if (!MeterReadValueIsValid(reading.MeterReadValue))
            {
                reading.ValidationErrors.Add("MeterReadValue must be in the format 'NNNNN'");
            }
            if (response.ValidReadings.Contains(reading))
            {
                //We have a duplicate, reject it
                reading.ValidationErrors.Add(@$"Duplicate meter reading found for AccountId: {reading.AccountId}, MeterReadingDateTime: {reading.MeterReadingDateTime}");
            }
        }

        public bool ValidateAccountExists(HashSet<int> missingAccountIds, int accountId, List<MeterReadingDTO> readings, AddMeterReadingsResultDTO result)
        {
            if (missingAccountIds.Contains(accountId))
            {
                readings.ForEach(reading =>
                {
                    reading.ValidationErrors.Add($"Account not found for AccountId {accountId}");
                    result.ValidReadings.Remove(reading);
                    result.InvalidReadings.Add(reading);
                });
                return false;
            }
            return true;
        }

        private bool MeterReadValueIsValid(string meterReadValue)
        {
            return Regex.IsMatch(meterReadValue, "^\\d{5}$");
        }

        private HashSet<int> GetMissingAccountIds(IEnumerable<int> accountIds)
        {
            var missingAccounts = new HashSet<int>(accountIds.Where(x => !_accountRepository.GetAccounts().Any(acc => acc.AccountId == x)));
            return missingAccounts;
        }
    }
}
