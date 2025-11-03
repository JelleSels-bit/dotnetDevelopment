using WebAPIDemo.Data.Repository;

namespace WebAPIDemo.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        IBestellingRepository BestellingRepository { get; }
	    IKlantRepository KlantRepository { get; }
        IProductRepository ProductRepository { get; }

	    public void SaveChanges();
    }
}
