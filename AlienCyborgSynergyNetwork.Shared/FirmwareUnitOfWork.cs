using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AlienCyborgSynergyNetwork.Shared
{
    public class FirmwareUnitOfWork : IFirmwareUnitOfWork
    {
        private readonly FirmwareDBContext _context;
        public IFirmwareRepository Firmware { get; }

        public FirmwareUnitOfWork(FirmwareDBContext context)
        {
            _context = context;
            Firmware = new FirmwareRepository(context);
        }

        public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();

        public void Dispose() => _context.Dispose();
    }
}
