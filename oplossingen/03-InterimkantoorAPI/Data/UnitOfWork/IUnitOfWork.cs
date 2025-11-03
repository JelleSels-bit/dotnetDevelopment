using InterimkantoorAPI.Data.Repositories;

namespace InterimkantoorAPI.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
    IKlantRepository KlantRepository { get; }​
    IJobRepository JobRepository { get; }
	Task SaveChangesAsync();​
    }
}
