using DogRescue.Models;
using Microsoft.EntityFrameworkCore;

namespace DogRescue.Data
{
    public class HondenRescueContext : DbContext
    {
        public HondenRescueContext(DbContextOptions<HondenRescueContext> options) : base(options) { }

        public DbSet<HondModel> Hond { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<HondModel>().ToTable("Hond");
        }
    }
}
