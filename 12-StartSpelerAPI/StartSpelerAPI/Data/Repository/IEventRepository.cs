namespace StartSpelerAPI.Data.Repository
{
    public interface IEventRepository : IGenericRepository<Event>
    {
        Task<IEnumerable<Event>> GetEventsWithCommunity();
        Task<Event?> GetEventWithCommunity(int id);
    }
}
