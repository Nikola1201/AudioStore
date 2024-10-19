using AudioStore.DataAccess.Repository.IRepository;
using AudioStore.Models;

namespace AudioStore.DataAccess.Repository.IRepository
{
    public interface IProductRepository:IRepository<Product>
    { 
        public Product UpdateProduct(Product product);

    }
}
