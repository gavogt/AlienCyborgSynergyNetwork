using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlienCyborgSynergyNetwork.Shared
{
    public class FirmwareDBContextFactory : IDesignTimeDbContextFactory<FirmwareDBContext>
    {
        public FirmwareDBContext CreateDbContext(string[] args)
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var dbPath = Path.Combine(folder, "firmware.db");

            Directory.CreateDirectory(folder);

            var opts = new DbContextOptionsBuilder<FirmwareDBContext>()
                .UseSqlite($"Data Source={dbPath}")
                .Options;

            return new FirmwareDBContext(opts);
        }
    }
}
