using AudioStore.DataAccess.Repository.IRepository;
using AudioStore.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace AudioStore.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db):base(db)
        {
           _db = db;
        }

        public Product UpdateProduct(Product product)
        {
            _db.Update(product);
            return product;
        }

    }
}
