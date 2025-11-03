using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace StartspelerAPI.Data
{
    public class StartspelerAPIContext : IdentityDbContext<IdentityUser>
    {
        public StartspelerAPIContext(DbContextOptions<StartspelerAPIContext> options) : base(options) { }

        public DbSet<Community> Communitys { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Inschrijving> Inschrijvingen { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
         {
          base.OnModelCreating(modelBuilder);
          modelBuilder.Entity<Community>().ToTable("Community");
          modelBuilder.Entity<Event>().ToTable("Event").Property(x => x.Prijs).HasColumnType("decimal(18,2)");
          modelBuilder.Entity<Inschrijving>().ToTable("Inschrijving");

            modelBuilder.Entity<Event>()
            .HasOne(x => x.Community)
            .WithMany(x => x.Events)
            .HasForeignKey(x => x.CommunityId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Inschrijving>()
            .HasOne(x => x.Gebruiker)
            .WithMany(x => x.Inschrijvingen)
            .HasForeignKey(x => x.GebruikerId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Inschrijving>()
            .HasOne(x => x.Event)
            .WithMany(x => x.Inschrijvingen)
            .HasForeignKey(x => x.EventId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Inschrijving>()
                .HasKey(x => new { x.EventId, x.GebruikerId });
         }


        


    }
}
