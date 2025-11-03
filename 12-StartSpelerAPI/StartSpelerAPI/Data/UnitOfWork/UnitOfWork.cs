

namespace StartSpelerAPI.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StartspelerAPIContext _context;
        private ICommunityRepository _communityRepository;
        private IInschrijvingRepository _inschrijvingRepository;
        private IEventRepository _eventRepository;

        public UnitOfWork(StartspelerAPIContext context)
        {
            _context = context;
        }

        public ICommunityRepository CommunityRepository
        {
            get
            {
                return _communityRepository ??= new CommunityRepository(_context);
            }
        }

        public IInschrijvingRepository InschrijvingRepository
        {
            get
            {
                return _inschrijvingRepository ??= new InschrijvingRepository(_context);
            }
        }
        public IEventRepository EventRepository
        {
            get
            {
                return _eventRepository ??= new EventRepository(_context);
            }
        }


        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
