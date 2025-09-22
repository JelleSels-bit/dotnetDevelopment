namespace APIdemo.Data
{
    public class APIdemoContext : DbContext
    {
        public APIdemoContext(DbContextOptions<APIdemoContext> options) : base(options)
        {
        
        }        

        public DbSet<Product> Producten {  get; set; }
        public DbSet<Pokemon> Pokemons { get; set; }

        //Overschrijven van de Producten & Pokemons naar de conventies van een db ergo geen cijfers en enkelvoud
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Product>().ToTable("Product");
            modelBuilder.Entity<Pokemon>().ToTable("Pokemon");
        }
    }
}
