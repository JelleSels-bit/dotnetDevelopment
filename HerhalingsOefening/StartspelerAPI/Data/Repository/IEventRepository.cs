namespace StartspelerAPI.Data.Repository
{
    public interface IEventRepository : IGenericRepository<Event>
    {
        Task<IEnumerable<Event>> GetAllEventsAsync();
        Task<Event?> GetEventByIdAsync(int id);
    }
}
