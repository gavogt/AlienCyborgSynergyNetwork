using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AlienCyborgSynergyNetwork.Shared
{
    public class FirmwareRepository : IFirmwareRepository
    {
        private readonly FirmwareDBContext _context;

        public FirmwareRepository(FirmwareDBContext context) => _context = context;

        public Task AddAsync(FirmwareImage image)
        {
            return _context.FirmwareImages.AddAsync(image).AsTask();
        }

        public Task<FirmwareImage?> GetLatestAsync()
        {
            return _context.FirmwareImages
                .OrderByDescending(f => f.Created)
                .FirstOrDefaultAsync();

        }
    }
}
