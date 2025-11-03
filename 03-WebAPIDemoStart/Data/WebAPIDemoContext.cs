using Microsoft.EntityFrameworkCore;
using WebAPIDemo.Models;

namespace WebAPIDemo.Data
{
    public class WebAPIDemoContext: DbContext
    {
        public WebAPIDemoContext(DbContextOptions<WebAPIDemoContext> options) : base(options) { }

        public DbSet<Product> Producten {  get; set; }
        public DbSet<Klant> Klanten {  get; set; }
        public DbSet<Bestelling> Bestellingen {  get; set; }
        public DbSet<Orderlijn> Orderlijnen {  get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Klant>().ToTable("Klant");
            modelBuilder.Entity<Bestelling>().ToTable("Bestelling");
            modelBuilder.Entity<Orderlijn>().ToTable("Orderlijn");
            modelBuilder.Entity<Product>().ToTable("Product").Property(p=>p.Prijs).HasColumnType("decimal(18,2)");


            //Relaties moeten enkel bij de veel kant beschreven worden.
            modelBuilder.Entity<Bestelling>()
                .HasOne(x => x.Klant)
                .WithMany(b => b.Bestellingen)
                .HasForeignKey(x => x.KlantId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Orderlijn>().HasOne(x => x.Bestelling)
                .WithMany(o => o.Orderlijnen)
                .HasForeignKey(f => f.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
                

        }
    }
}
