using AudioStore.DataAccess;
using AudioStore.Models;
using AudioStore.Models.ViewModels;
using AudioStore.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioStore.Services
{
    public class ProductServices : IProductService
    {
        public ApplicationDbContext Context { get; }
        public ProductServices(ApplicationDbContext _context)
        {
            Context = _context;
        }
        public async Task<Product> CreateProduct(Product product)
        {
            Context.Products.Add(product);
            await Context.SaveChangesAsync();
            return product;
        }

        public async Task DeleteProduct(int? id)
        {
            var product = await Context.Products.FindAsync(id);
            if (product != null)
            {
                Context.Products.Remove(product);
                await Context.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException($"Product with ID {id} doesn't exist!");
            }
        }

        public async Task<List<Product>> GetAllProducts()
        {
           return await Context.Products.ToListAsync();
        }

        public Task<Product> GetProductById(int? id)
        {
            return Context.Products.FirstOrDefaultAsync(x => x.ProductID == id);
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            Context.Update(product);
            await Context.SaveChangesAsync();
            return product;
        }
        public async Task<List<ProductVM>> GetProductVMs()
        {
            var productVMs = await (from p in Context.Products
                                    join c in Context.Categories on p.CategoryID equals c.CategoryID
                                    join m in Context.Manufacturers on p.ManufacturerID equals m.ManufacturerID
                                    select new ProductVM
                                    {
                                        ProductID = p.ProductID,
                                        Name = p.Name,
                                        CategoryName = c.Name,
                                        ManufacturerName = m.Name,
                                        Price = p.Price
                                    }).ToListAsync();

            return productVMs;
        }
    }
}
