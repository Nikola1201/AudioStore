using AudioStore.DataAccess;
using AudioStore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioStore.Services
{
    public class CartService
    {
       private readonly ApplicationDbContext _context;
        public CartService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task  CreateCart(ShoppingCart cart)
        {
          
            _context.Entry(cart.Product).State = EntityState.Unchanged;
            _context.Entry(cart.Product.Manufacturer).State = EntityState.Unchanged;
            _context.Entry(cart.Product.Category).State = EntityState.Unchanged;
    
            _context.Add(cart);
            await _context.SaveChangesAsync();
        }
    }
}
