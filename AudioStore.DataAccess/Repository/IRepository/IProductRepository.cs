using AudioStore.DataAccess.Repository.IRepository;
using AudioStore.Models;

namespace AudioStore.DataAccess.Repository.IRepository
{
    public interface IProductRepository:IRepository<Product>
    { 
        public Task<Product> UpdateProduct(Product product);

    }
}
