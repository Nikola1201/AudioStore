using AudioStore.Models;
using Microsoft.EntityFrameworkCore;

namespace AudioStore.DataAccess.Repository
{
    public class CartItemRepository:Repository<ShoppingCartItem>
    {
       private readonly ApplicationDbContext _db;
        public CartItemRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }
        public async Task  CreateCart(ShoppingCartItem cart)
        {
          
            _db.Entry(cart.Product).State = EntityState.Unchanged;
            _db.Entry(cart.Product.Manufacturer).State = EntityState.Unchanged;
            _db.Entry(cart.Product.Category).State = EntityState.Unchanged;
    
            _db.Add(cart);
            await _db.SaveChangesAsync();
        }
    }
}
