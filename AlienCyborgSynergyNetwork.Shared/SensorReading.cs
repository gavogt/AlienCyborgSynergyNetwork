using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlienCyborgSynergyNetwork
{
    public class SensorReading
    {
        public Guid ID { get; set; }
        public DateTime Timestamp { get; set; }
        public string Topic { get; set; } = default!;
        public string Payload { get; set; } = default!;

    }
}
