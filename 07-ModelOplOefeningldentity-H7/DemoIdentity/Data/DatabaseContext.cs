using DemoIdentity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Numerics;
using System.Reflection.Emit;

namespace DemoIdentity.Data
{
    public class DatabaseContext : IdentityDbContext<CustomUser>
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        public DbSet<Klant> Klanten { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Klant>().ToTable("Klant");

        }
        public DbSet<DemoIdentity.Models.Bestelling> Bestelling { get; set; } = default!;
    }
}