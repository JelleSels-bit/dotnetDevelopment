
using Microsoft.EntityFrameworkCore;

namespace StartspelerAPI.Data.Repository
{
    public class EventRepository : GenericRepository<Event>, IEventRepository
    {
        public EventRepository(StartspelerAPIContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Event>> GetAllEventsAsync()
        {
            return await _context.Events.Include(x => x.Community).ToListAsync();
        }

        public async Task<Event?> GetEventByIdAsync(int id)
        {
            return await _context.Events.Include(x => x.Community).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
