
using Microsoft.EntityFrameworkCore;

namespace StartSpelerAPI.Data
{
    public class StartspelerAPIContext : IdentityDbContext<Gebruiker>
    {
        public StartspelerAPIContext(DbContextOptions<StartspelerAPIContext>
            options) : base(options) { }

        public DbSet<Community> Communities { get; set; }
        public DbSet<Inschrijving> Inschrijvingen { get; set; }
        public DbSet<Event> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Community>().ToTable("Community");
            modelBuilder.Entity<Inschrijving>().ToTable("Inschrijving");
            modelBuilder.Entity<Event>().ToTable("Event")
                .Property(x => x.Prijs).HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Event>()
                .HasOne(x => x.Community)
                .WithMany(x => x.Events)
                .HasForeignKey(x => x.CommunityId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Inschrijving>()
                .HasOne(x => x.Event)
                .WithMany(x => x.Inschrijvingen)
                .HasForeignKey(x => x.EventId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Inschrijving>()
                .HasKey(x => new { x.EventId, x.GebruikerId });

            modelBuilder.Entity<Inschrijving>()
                .HasOne(x => x.Gebruiker)
                .WithMany(x => x.Inschrijvingen)
                .HasForeignKey(x => x.GebruikerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
