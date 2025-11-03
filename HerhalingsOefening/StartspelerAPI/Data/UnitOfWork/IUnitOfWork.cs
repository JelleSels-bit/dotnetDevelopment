using StartspelerAPI.Data.Repository;

namespace StartspelerAPI.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        ICommunityRepository CommunityRepository { get; }
        IEventRepository EventRepository { get; }
        IInschrijvingRepository InschrijvingRepository { get; }
        public Task SaveChangesAsync();
            
        
    }
}
