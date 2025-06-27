using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlienCyborgSynergyNetwork.Shared
{
    public class RatingDBContext : DbContext
    {
        public DbSet<Rating> Ratings { get; set; } = null!;
    }
}
