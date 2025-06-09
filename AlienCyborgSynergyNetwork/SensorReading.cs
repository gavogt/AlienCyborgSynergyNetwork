using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlienCyborgSynergyNetwork
{
    public class SensorReading
    {
        public int ID { get; set; }
        public Guid SessionId { get; set; }
        public DateTime Timestamp { get; set; }
        public string Topic { get; set; } = default!;
        public string Payload { get; set; } = default!;

        public CyborgSession Session { get; set; } = default!;

    }
}
