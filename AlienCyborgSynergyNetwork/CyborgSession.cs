using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlienCyborgSynergyNetwork
{
    public record CyborgSession
    (
            Guid Id,
            string UnitId,
            SessionType Type,
            DateTime StartTime,
            DateTime EndTime,
            string Metadata
    );
}
