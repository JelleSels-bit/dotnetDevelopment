using InterimkantoorAPI.Data.Repositories;

namespace InterimkantoorAPI.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly InterimkantoorAPIContext _context;

    
        private IKlantRepository _klantRepository;
        private IJobRepository _jobRepository;

        public UnitOfWork(InterimkantoorAPIContext context)
        {
            _context = context;
        }

        public IKlantRepository KlantRepository => _klantRepository ??= new KlantRepository(_context);
        public IJobRepository JobRepository => _jobRepository ??= new JobRepository(_context);

        

        public Task SaveChangesAsync()
        {
            throw new NotImplementedException();
        }
    }
}
