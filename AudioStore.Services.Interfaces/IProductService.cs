using AudioStore.Models;
using AudioStore.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioStore.Services.Interfaces
{
    public interface IProductService
    {
        public Task<Product> CreateProduct(Product product);
        public Task<List<Product>> GetAllProducts();
        public Task<Product> GetProductById(int? id);
        public Task<Product> UpdateProduct(Product product);
        public Task DeleteProduct(int? id);
    
    }
}
