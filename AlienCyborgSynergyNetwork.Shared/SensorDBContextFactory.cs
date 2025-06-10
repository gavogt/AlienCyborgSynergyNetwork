using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace AlienCyborgSynergyNetwork.Shared
{
    public class SensorDBContextFactory : IDesignTimeDbContextFactory<SensorDBContext>
    {
        public SensorDBContext CreateDbContext(string[] args)
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var dbPath = Path.Combine(folder, "sensor.db");

            Directory.CreateDirectory(folder);

            var opts = new DbContextOptionsBuilder<SensorDBContext>()
            .UseSqlite($"Data Source={dbPath}")
            .Options;

            return new SensorDBContext(opts);
        }
    }
}
