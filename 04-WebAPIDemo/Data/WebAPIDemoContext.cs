using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using WebAPIDemo.Models;

namespace WebAPIDemo.Data
{
    public class WebAPIDemoContext: IdentityDbContext<CustomUser>
    {
        public WebAPIDemoContext(DbContextOptions<WebAPIDemoContext> options) : base(options) { }

        public DbSet<Product> Producten {  get; set; }
        public DbSet<Klant> Klanten {  get; set; }
        public DbSet<Bestelling> Bestellingen {  get; set; }
        public DbSet<Orderlijn> Orderlijnen {  get; set; }

        public DbSet<Functie> Functies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Klant>().ToTable("Klant");
            modelBuilder.Entity<Functie>().ToTable("Functie");
            modelBuilder.Entity<Bestelling>().ToTable("Bestelling");
            modelBuilder.Entity<Orderlijn>().ToTable("Orderlijn");
            modelBuilder.Entity<Product>().ToTable("Product").Property(p=>p.Prijs).HasColumnType("decimal(18,2)");

            // One to many 
            modelBuilder.Entity<Bestelling>()
                .HasOne(p => p.Klant)
                .WithMany(x => x.Bestellingen)
                .HasForeignKey(y => y.KlantId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            // Many to many 
            modelBuilder.Entity<Orderlijn>()
                .HasOne(p => p.Bestelling)
                .WithMany(x => x.Orderlijnen)
                .HasForeignKey(y => y.BestellingId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            modelBuilder.Entity<Orderlijn>()
                .HasOne(p => p.Product)
                .WithMany(x => x.Orderlijnen)
                .HasForeignKey(y => y.ProductId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();
        }
    }
}
