namespace StartSpelerAPI.Data.Repository
{
    public interface ICommunityRepository : IGenericRepository<Community>
    {
        Task<IEnumerable<Community>> GetCommunitiesWithEvents();
        Task<Community?> GetCommunityWithEvents(int id);
    }
}
