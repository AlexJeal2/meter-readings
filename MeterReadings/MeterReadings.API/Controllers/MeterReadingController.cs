using MeterReadings.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace MeterReadings.API.Controllers
{
    [ApiController]
    public class MeterReadingController : ControllerBase
    {
        private readonly ILogger<MeterReadingController> _logger;
        private readonly IMeterReadingService _meterReadingService;

        public MeterReadingController(IMeterReadingService meterReadingService, ILogger<MeterReadingController> logger)
        {
            _logger = logger;
            _meterReadingService = meterReadingService;
        }


        [HttpPost("api/meter-reading-uploads")]
        public async Task<IActionResult> UploadMeterReadings(IFormFile readings)
        {
            var readingsCsv = await ReadCsvFromFile(readings);

            var meterReadings = await _meterReadingService.AddMeterReadingsFromCsvAsync(readingsCsv);

            return Created(string.Empty, new
            {
                SuccessCount = meterReadings.ValidReadings.Count,
                FailureCount = meterReadings.InvalidReadings.Count,
                FailedReadings = meterReadings.InvalidReadings
            });
        }

        private async Task<string> ReadCsvFromFile(IFormFile file)
        {
            var result = new StringBuilder();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                    result.AppendLine(await reader.ReadLineAsync());
            }
            return result.ToString();
        }
    }
}
