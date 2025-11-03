using System.Collections;

namespace InterimkantoorAPI.Data.Repositories
{
    public interface IJobRepository : IGenericRepository<Job>
    {
        Task<IEnumerable<Job>> GetJobs();
        Task<Job> GetJob(int id);
        Task<IEnumerable<Job>> SearchAsync(string zoekwaarde);
    }
}
