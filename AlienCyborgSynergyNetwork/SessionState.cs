using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlienCyborgSynergyNetwork
{
    public class SessionState
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public SessionState()
        {
            
        }
    }
}
