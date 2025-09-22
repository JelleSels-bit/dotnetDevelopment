using InterimkantoorAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InterimkantoorAPI.Data
{
    public class APIinterimkantoorContext : DbContext
    {
        public APIinterimkantoorContext(DbContextOptions<APIinterimkantoorContext> options) : base(options)
        {
        
        }

        public DbSet<Job> Jobs { get; set; }
        public DbSet<Klant> Klanten { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Job>().ToTable("Job");
            modelBuilder.Entity<Klant>().ToTable("Klant");
        }
    }   
}
