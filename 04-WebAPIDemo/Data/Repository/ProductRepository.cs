
using Microsoft.EntityFrameworkCore;

namespace WebAPIDemo.Data.Repository
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(WebAPIDemoContext context) : base(context) { }

        public async Task<IEnumerable<Product>> SearchAsync(string zoekwaarde)
        {
            return await _context
                .Producten
                .Where(x => x.Naam.Contains(zoekwaarde))
                .OrderBy(x => x.Naam)
                .ToListAsync();
        }
    }
}
