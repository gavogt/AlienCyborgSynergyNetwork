using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AlienCyborgSynergyNetwork.Shared
{
    public class SensorRepository:ISensorRepository
    {
        private readonly SensorDBContext _context;
        public SensorRepository(SensorDBContext context)
        {
            _context = context;
        }

        public async Task AddAsync(SensorReading reading) => await _context.Sensors.AddAsync(reading);

        public async Task<IEnumerable<SensorReading>> ListAsync() => await _context.Sensors.ToListAsync();
    }
}
