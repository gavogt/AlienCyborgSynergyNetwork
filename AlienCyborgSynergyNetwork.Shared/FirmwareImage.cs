using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlienCyborgSynergyNetwork.Shared
{
    public class FirmwareImage
    {
        public int ID { get; set; }
        public string Version { get; set; } = default!;
        public string FileName { get; set; } = default!;
        public DateTime Created { get; set; }

    }
}
