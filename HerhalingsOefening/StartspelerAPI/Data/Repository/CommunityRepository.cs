using Microsoft.EntityFrameworkCore;
using StartspelerAPI.DTO.Community;

namespace StartspelerAPI.Data.Repository
{
    public class CommunityRepository : GenericRepository<Community>, ICommunityRepository
    {
        public CommunityRepository(StartspelerAPIContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Community>> GetAllCommunityAsync()
        {
            return await _context.Communitys.Include(x => x.Events).ToListAsync();
        }

        public async Task<Community?> GetCommunityByIdAsync(int id)
        {
            return await _context.Communitys.Include(x => x.Events).FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
