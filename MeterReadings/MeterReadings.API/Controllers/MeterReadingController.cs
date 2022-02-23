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


        [HttpPost("/meter-reading-uploads")]
        public async Task<IActionResult> UploadMeterReadings(IFormFile readings)
        {
            var readingsCsv = await ReadCsvFromFile(readings);

            _meterReadingService.GetMeterReadingsFromCsv(readingsCsv);

            return Ok();
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
