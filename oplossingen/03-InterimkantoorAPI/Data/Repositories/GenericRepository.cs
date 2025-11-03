using System.Formats.Tar;
using Microsoft.EntityFrameworkCore;

namespace InterimkantoorAPI.Data.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {

        protected readonly InterimkantoorAPIContext _context;

        public GenericRepository(InterimkantoorAPIContext context)
        {
            _context = context;
        }

        
        public async Task AddAsync(TEntity entity)
        {
            await _context.Set<TEntity>().AddAsync(entity);

        }

        public async Task<TEntity?> FindAsync<T>(T id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public async void Delete(TEntity entity)
        {
	        _context.Set<TEntity>().Remove(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _context.Set<TEntity>().ToListAsync();​

        }

        public async Task<TEntity?> GetByIdAsync(string id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public async void Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
        }
    }
}​
    
