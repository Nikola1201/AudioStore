using AudioStore.DataAccess;
using AudioStore.DataAccess.Repository;
using AudioStore.DataAccess.Repository.IRepository;

using AudioStore.Services.Interfaces;


namespace AudioStore.Services
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryRepository(_db);
            Manufacturer = new ManufacturerRepository(_db);
            Product = new ProductRepository(_db);
            Customer = new CustomerRepository(_db);
            OrderDetails = new OrderDetailsRepository(_db);



        }
        public ICategoryRepository Category { get; private set; }
        public IManufacturerRepository Manufacturer { get; private set; }
        public IProductRepository Product { get; private set; }
        public ICustomerRepository Customer { get; private set; }
        public IOrderDetailsRepository OrderDetails { get; private set; }



       public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
