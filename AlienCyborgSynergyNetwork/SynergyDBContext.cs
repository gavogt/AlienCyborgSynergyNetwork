using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AlienCyborgSynergyNetwork
{
    internal class SynergyDBContext : DbContext
    {

        public SynergyDBContext(DbContextOptions<SynergyDBContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<SessionState> Sessions { get; set; }

    }
}
