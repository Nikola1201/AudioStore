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
           IQueryable<Product> query = Context.Products;
            query=query.Include(p => p.Category).Include(p => p.Manufacturer);
            return await query.ToListAsync();
        }

        public Task<Product> GetProductById(int? id)
        {
          IQueryable<Product> query = Context.Products;
            query = query.Include(p => p.Category).Include(p => p.Manufacturer);
            return query.FirstOrDefaultAsync(p => p.ProductID == id);
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            Context.Update(product);
            await Context.SaveChangesAsync();
            return product;
        }
   
    }
}
