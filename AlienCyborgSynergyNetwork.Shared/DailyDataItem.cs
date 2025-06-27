using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlienCyborgSynergyNetwork.Shared
{
    public class DailyDataItem
    {
        public string Date { get; set; } = "";
        public double Stability { get; set; }
        public double Efficiency { get; set; }
        public double Cohesion { get; set; }
    }
}
