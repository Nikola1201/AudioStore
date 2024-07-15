using AudioStore.DataAccess;
using AudioStore.Models;
using AudioStore.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace AudioStore.Services
{
    public class OrderDetailsService : IOrderDetailsService
    {
        private readonly ApplicationDbContext _context;

        public OrderDetailsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateOrderDetails(OrderDetails orderDetails)
        {

            foreach (var cartItem in orderDetails.Carts)
            {
                _context.Entry(cartItem.Product).State = EntityState.Unchanged;
                _context.Entry(cartItem.Product.Manufacturer).State = EntityState.Unchanged;
                _context.Entry(cartItem.Product.Category).State = EntityState.Unchanged;
            }
            _context.Add(orderDetails);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                foreach (var entry in ex.Entries)
                {
                    await entry.ReloadAsync();
                }
               
            }
        }

        public async Task DeleteOrderDetails(int? id)
        {
            var order = await _context.OrderDetails.FindAsync(id);
            if (order != null)
            {
                _context.OrderDetails.Remove(order);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException($"Order detail with {id} doesn't exist");
            }
        }
        public async Task<int> GetOrderDetailsID()
        {
            var hasOrderDetails = await _context.OrderDetails.AnyAsync();
            if (!hasOrderDetails)
            {
                
                return 0;
            }
            return await _context.OrderDetails.MaxAsync(od => od.OrderID);
        }
    }
}
