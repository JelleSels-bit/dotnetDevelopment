using WebAPIDemo.Data.Repository;

namespace WebAPIDemo.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly WebAPIDemoContext _context;

        private IBestellingRepository bestellingRepository;
        private IKlantRepository klantRepository;
        private IProductRepository productRepository;

        public UnitOfWork(WebAPIDemoContext context)
        {
            _context = context;
        }

        public IBestellingRepository BestellingRepository
        {
            get 
            {
                if (this.bestellingRepository == null)
				this.bestellingRepository
                    = new BestellingRepository(_context);
                return bestellingRepository;
		    }
        }

        public IKlantRepository KlantRepository
        {
            get
            {
                if (this.klantRepository == null)
				this.klantRepository
                    = new KlantRepository(_context);
			return klantRepository;
		    }
        }

        public IProductRepository ProductRepository
        {
            get
            {
                if (this.productRepository == null)
				this.productRepository
                    = new ProductRepository(_context);
                return productRepository​;
		    }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
