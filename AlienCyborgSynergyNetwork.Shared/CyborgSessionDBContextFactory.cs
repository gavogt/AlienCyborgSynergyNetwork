using Microsoft.EntityFrameworkCore.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AlienCyborgSynergyNetwork
{
    internal class CyborgSessionDBContextFactory : IDesignTimeDbContextFactory<CyborgSessionDBContext>
    {
        public CyborgSessionDBContext CreateDbContext(string[] args)
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var dbPath = Path.Combine(folder, "cyborg.db");

            Directory.CreateDirectory(folder);

            Console.WriteLine($"▶︎ Design-time Cyborg Session DB path: {dbPath}");

            var opts = new DbContextOptionsBuilder<CyborgSessionDBContext>()
                .UseSqlite($"Data Source={dbPath}")
                .Options;

            return new CyborgSessionDBContext(opts);

        }
    }
}
