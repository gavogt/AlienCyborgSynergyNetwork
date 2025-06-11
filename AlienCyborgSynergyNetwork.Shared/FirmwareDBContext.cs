using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlienCyborgSynergyNetwork.Shared
{
    public class FirmwareDBContext : DbContext
    {
        public DbSet<FirmwareImage> FirmwareImages { get; set; } = null!;
    }
}
