using Microsoft.EntityFrameworkCore;

namespace StartSpelerAPI.Data.Repository
{
    public class EventRepository : GenericRepository<Event>, IEventRepository
    {
        public EventRepository(StartspelerAPIContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Event>> GetEventsWithCommunity()
        {
            return await _context.Set<Event>().Include(ev => ev.Community).ToListAsync();
        }

        public async Task<Event?> GetEventWithCommunity(int id)
        {
            return await _context.Set<Event>().Include(ev => ev.Community).FirstOrDefaultAsync(ev => ev.Id == id);
        }
    }
}
