using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AlienCyborgSynergyNetwork
{
    public class SynergyContextFactory:IDesignTimeDbContextFactory<SynergyDBContext>
    {
        public SynergyDBContext CreateDbContext(string[] args)
        {

            var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var dbPath = Path.Combine(folder, "synergy.db");
            Directory.CreateDirectory(folder);

            Console.WriteLine($"▶︎ Design-time DB path: {dbPath}");

            var opts = new DbContextOptionsBuilder<SynergyDBContext>()
                .UseSqlite($"Data Source={dbPath}")
                .Options;

            return new SynergyDBContext(opts);
        }
    }
}
