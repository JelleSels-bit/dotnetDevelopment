

namespace StartSpelerAPI.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        ICommunityRepository CommunityRepository { get; }
        IInschrijvingRepository InschrijvingRepository { get; }
        IEventRepository EventRepository { get; }

        public Task SaveChangesAsync();
    }
}
