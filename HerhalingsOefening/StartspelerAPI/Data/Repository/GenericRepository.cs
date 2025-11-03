using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace StartspelerAPI.Data.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly StartspelerAPIContext _context;

        public GenericRepository(StartspelerAPIContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TEntity entity)
        {
            try 
            { await _context.Set<TEntity>().AddAsync(entity); }

            catch (Exception e )
            {
                throw new Exception(e.Message);
            }
        }

        public void Delete(TEntity entity)
        {
            _context.Set<TEntity>().Remove(entity);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync<T>(T id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }

        public void Update(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);
        }
    }
}
