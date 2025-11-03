namespace StartSpelerAPI.Data.Repository
{
    public interface IInschrijvingRepository : IGenericRepository<Inschrijving>
    {
        Task<Inschrijving> GetInschrijving(int eventId, string gebruikerId);
        Task<IEnumerable<Inschrijving>> GetVolledigeInschrijvingen();
        Task<Inschrijving> GetVolledigeInschrijving(int it);
    }
}
