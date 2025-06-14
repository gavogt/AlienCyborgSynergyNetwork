﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AlienCyborgSynergyNetwork;

namespace AlienCyborgSynergyNetwork
{
    public class CyborgSession
    {
        public Guid Id { get; set; }
        public string UnitId { get; set; }
        public SessionType Type { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? Metadata { get; set; }

        public ICollection<SensorReading> SensorReadings { get; set; } = new List<SensorReading>();

    }
}
