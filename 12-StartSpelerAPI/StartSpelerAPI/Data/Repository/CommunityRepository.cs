
namespace StartSpelerAPI.Data.Repository
{
    public class CommunityRepository : GenericRepository<Community>, ICommunityRepository
    {
        public CommunityRepository(StartspelerAPIContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Community>> GetCommunitiesWithEvents()
        {
            return await _context.Set<Community>().Include(community => community.Events).ToListAsync();
        }

        public async Task<Community?> GetCommunityWithEvents(int id)
        {
            return await _context.Set<Community>().Include(community => community.Events).FirstOrDefaultAsync(community => community.Id == id);
        }
    }
}
