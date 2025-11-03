namespace InterimkantoorAPI.Data.Repositories
{
    public interface IKlantRepository : IGenericRepository<Klant> 
    {
        Task<IEnumerable<Klant>> SearchAsync(string zoekwaarde);
        
    }
}
