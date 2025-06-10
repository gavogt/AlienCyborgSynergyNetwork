using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlienCyborgSynergyNetwork.Shared
{
    public interface ISensorRepository
    {
        Task AddAsync(SensorReading reading);
        Task<IEnumerable<SensorReading>> ListAsync();
    }
}
