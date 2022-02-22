using Microsoft.AspNetCore.Mvc;

namespace MeterReadings.API.Controllers
{
    [ApiController]
    public class MeterReadingController : ControllerBase
    {
        private readonly ILogger<MeterReadingController> _logger;

        public MeterReadingController(ILogger<MeterReadingController> logger)
        {
            _logger = logger;
        }


        [HttpPost("/meter-reading-uploads")]
        public IActionResult UploadMeterReadings(IFormFile readings)
        {
            var f = readings;
            var result = new
            {
                Message = "Hello World"
            };

            return Ok(result);
        }
    }
}
