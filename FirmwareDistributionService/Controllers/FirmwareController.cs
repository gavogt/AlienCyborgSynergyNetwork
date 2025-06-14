using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using AlienCyborgSynergyNetwork.Shared;
using System.Threading.Tasks;

namespace FirmwareDistributionService.Controllers
{
    [ApiController]
    [Route("api/firmware")]
    public class FirmwareController : ControllerBase
    {
        [HttpGet("latest")]
        public async Task<IActionResult> GetLatest(
        
            [FromServices] IFirmwareUnitOfWork uow)
        
            {
            var fw = await uow.Firmware.GetLatestAsync();
                if (fw is null)
                {
                    return NotFound();
                }

                var url = Url.Content($"~/firmware/{fw.Version}/{fw.FileName}");

                return Redirect(url);
            } 

        [HttpPost]
        public async Task<IActionResult> Upload(
            [FromForm] string version,
            [FromForm] IFormFile firmware,
            [FromServices] IFirmwareUnitOfWork uow,
            [FromServices] IWebHostEnvironment env)
        {
            var folder = Path.Combine(env.ContentRootPath, "wwwroot", "firmware", version);

            Directory.CreateDirectory(folder);
    

            var path = Path.Combine(folder, firmware.FileName);

            await using var fs = System.IO.File.Create(path);

            await firmware.CopyToAsync(fs);

            var fw = new FirmwareImage
            {
                Version = version,
                FileName = firmware.FileName,
                Created = DateTime.UtcNow
            };

            await uow.Firmware.AddAsync(fw);

            await uow.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLatest), new { }, null);
        }
    }
}
