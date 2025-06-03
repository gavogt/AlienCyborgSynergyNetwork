using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlienCyborgSynergyNetwork
{
    public class CyborgSessionDBContext : DbContext
    {
        public DbSet<CyborgSession> CyborgSessions { get; set; }

        public CyborgSessionDBContext(DbContextOptions<CyborgSessionDBContext> options)
            : base(options)
        {
        }
    }
}
