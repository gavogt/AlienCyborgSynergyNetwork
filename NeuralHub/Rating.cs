using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hubs
{
    public class Rating
    {
        public Guid Id { get; set; }
        public Guid SessionId { get; set; }
        public int Stability { get; set; }
        public int Efficiency { get; set; }
        public int Cohesion { get; set; }
        public DateTime TimeStamp { get; set; }

    }
}
