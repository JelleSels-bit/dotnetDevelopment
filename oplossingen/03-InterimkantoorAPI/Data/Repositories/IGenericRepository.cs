namespace InterimkantoorAPI.Data.Repositories
{
    public interface IGenericRepository<TEntity> where TEntity : class 
    {
    Task<IEnumerable<TEntity>> GetAllAsync();​
	Task<TEntity?> GetByIdAsync(string id);​
	Task AddAsync(TEntity entity);​
	void Update(TEntity entity);​
	void Delete(TEntity entity);
    Task<TEntity?> FindAsync<T>(T id);
    }
    
    
}
