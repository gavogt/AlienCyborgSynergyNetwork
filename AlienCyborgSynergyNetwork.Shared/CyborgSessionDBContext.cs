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
        public DbSet<SensorReading> Readings { get; set; }

        public CyborgSessionDBContext(DbContextOptions<CyborgSessionDBContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CyborgSession>().HasKey(s => s.Id);
            modelBuilder.Entity<SensorReading>()
                .HasKey(r => r.ID);
        }
    }
}
