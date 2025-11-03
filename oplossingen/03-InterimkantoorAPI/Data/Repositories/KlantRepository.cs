
namespace InterimkantoorAPI.Data.Repositories
{
    public class KlantRepository : GenericRepository<Klant>, IKlantRepository
    {
        public KlantRepository(InterimkantoorAPIContext context) : base(context) 
        {
        }

        public async Task<IEnumerable<Klant>> SearchAsync(string zoekwaarde)
        {
            return await _context.Klanten.Where(x => x.Naam.Contains(zoekwaarde))
                .OrderBy(x => x.Naam)
                .ToListAsync();
        }
    }
}
