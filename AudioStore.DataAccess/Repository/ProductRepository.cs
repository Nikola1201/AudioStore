using AudioStore.DataAccess.Repository.IRepository;
using AudioStore.Models;

namespace AudioStore.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db):base(db)
        {
           _db = db;
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            _db.Update(product);
            await _db.SaveChangesAsync();
            return product;
        }
    }
}
