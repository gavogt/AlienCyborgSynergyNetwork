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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={Path.Combine(FileSystem.AppDataDirectory, "cyborg.db")}");
        }
    }
}
