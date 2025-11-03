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
        public DbSet<KlantJob> KlantJob { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Job>().ToTable("Job");
            modelBuilder.Entity<Klant>().ToTable("Klant");
           

            // Required relatie - standaard Cascade
            modelBuilder.Entity<KlantJob>()
                .HasOne(o => o.Klant)
                .WithMany(c => c.KlantJobs)
                .HasForeignKey(o => o.KlantId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();


            modelBuilder.Entity<KlantJob>()
                .HasOne(o => o.Job)
                .WithMany(c => c.KlantJobs)
                .HasForeignKey(o => o.JobId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
        }
    }   
}
