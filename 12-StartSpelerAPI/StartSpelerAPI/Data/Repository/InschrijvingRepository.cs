
using StartSpelerAPI.Models;

namespace StartSpelerAPI.Data.Repository
{
    public class InschrijvingRepository : GenericRepository<Inschrijving>, IInschrijvingRepository
    {
        public InschrijvingRepository(StartspelerAPIContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Inschrijving>> GetVolledigeInschrijvingen()
        {
            return await _context.Inschrijvingen.ToListAsync();
        }

        public async Task<Inschrijving> GetVolledigeInschrijving(int id)
        {
            return await _context.Inschrijvingen
                .Include(inschrijving => inschrijving.Gebruiker)
                .Include(inschrijving => inschrijving.Event)
                .FirstOrDefaultAsync(inschrijving => inschrijving.Id == id);
        }

        public async Task<Inschrijving> GetInschrijving(int eventId, string gebruikerId)
        {
            return await _context.Inschrijvingen
                .Where(inschrijving => inschrijving.EventId == eventId)
                .Where(inschrijving => inschrijving.GebruikerId == gebruikerId)
                .FirstOrDefaultAsync();
        }
    }
}
