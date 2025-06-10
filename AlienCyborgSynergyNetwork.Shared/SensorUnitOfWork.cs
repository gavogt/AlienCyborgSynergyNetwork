using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlienCyborgSynergyNetwork.Shared
{
    public class SensorUnitOfWork : ISensorUnitOfWork
    {
        private readonly SensorDBContext _context;
        public ISensorRepository Sensors { get; }

        public SensorUnitOfWork(SensorDBContext context)
        {
            _context = context;
            Sensors = new SensorRepository(context);
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
