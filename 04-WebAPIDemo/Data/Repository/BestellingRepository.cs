namespace WebAPIDemo.Data.Repository
{
    public class BestellingRepository : GenericRepository<Bestelling>, IBestellingRepository
    {
        public BestellingRepository(WebAPIDemoContext context) : base(context) { }
    }
}
