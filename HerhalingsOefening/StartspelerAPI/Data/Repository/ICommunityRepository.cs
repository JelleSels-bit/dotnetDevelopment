using StartspelerAPI.DTO.Community;

namespace StartspelerAPI.Data.Repository
{
    public interface ICommunityRepository: IGenericRepository<Community>
    {
        Task<IEnumerable<Community>> GetAllCommunityAsync();
        Task<Community?> GetCommunityByIdAsync(int id);
        
    }
}
