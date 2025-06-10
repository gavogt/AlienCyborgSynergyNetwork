using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlienCyborgSynergyNetwork.Shared
{
    public class SensorDBContext : DbContext
    {
        public DbSet<SensorReading> Sensors { get; set; } = null!;

        public SensorDBContext(DbContextOptions<SensorDBContext> opts)
            : base(opts)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SensorReading>()
                        .ToTable("sensors")
                        .HasKey(r => r.ID);
        }
    }
}
