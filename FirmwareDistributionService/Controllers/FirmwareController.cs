using Microsoft.AspNetCore.Mvc;

namespace FirmwareDistributionService.Controllers
{
    [ApiController]
    [Route("api/firmware")]
    public class FirmwareController : ControllerBase
    {
        [HttpGet("latest")]
        public IActionResult GetLatest()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "firmware.bin");

            if (!System.IO.File.Exists(path))
            {
                return NotFound("Firmware file not found.");
            }

            return PhysicalFile(path, "application/octet-stream", "firmware.bin");

        }
    }
}
