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

        public RatingDBContext(DbContextOptions<RatingDBContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Rating>()
                .HasKey(r => r.Id);

            modelBuilder.Entity<Rating>()
                .Property(r => r.SessionId)
                .IsRequired();

            modelBuilder.Entity<Rating>()
                .Property(r => r.Stability)
                .IsRequired();

            modelBuilder.Entity<Rating>()
                .Property(r => r.Efficiency)
                .IsRequired();

            modelBuilder.Entity<Rating>()
                .Property(r => r.Cohesion)
                .IsRequired();

            modelBuilder.Entity<Rating>()
                .Property(r => r.TimeStamp)
                .IsRequired();

        }
    }
}
