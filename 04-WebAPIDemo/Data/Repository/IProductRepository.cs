namespace WebAPIDemo.Data.Repository
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> SearchAsync(string zoekwaarde);
    }
}
