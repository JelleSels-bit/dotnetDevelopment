namespace WebAPIDemo.Data.Repository
{
    public class KlantRepository : GenericRepository<Klant>, IKlantRepository
    {
        public KlantRepository(WebAPIDemoContext context) : base(context) { }
    }
}
