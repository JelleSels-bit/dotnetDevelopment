
namespace InterimkantoorAPI.Data.Repositories
{
    public class JobRepository : GenericRepository<Job>, IJobRepository
    {
        public JobRepository(InterimkantoorAPIContext context) : base(context)
        {

        }

        public async Task<IEnumerable<Job>> SearchAsync(string zoekwaarde)
        {
            return await _context.Jobs.Where(x => x.Omschrijving.Contains(zoekwaarde))
                .Include(x => x.KlantJobs)
                .ThenInclude(x => x.Klant)
                .OrderBy(x => x.Omschrijving)
                .ToListAsync();
        }

        public async Task<Job> GetJob(int id)
        {
            return await _context.Jobs.Include(x => x.KlantJobs).ThenInclude(x => x.Klant).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Job>> GetJobs()
        {
            return await _context.Jobs.Include(x => x.KlantJobs).ThenInclude(x => x.Klant).ToListAsync();
        }

        
    }
}
