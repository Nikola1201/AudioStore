using AudioStore.Models;

namespace AudioStore.Services.Interfaces
{
    public interface IShoppingCartService
    {
        public Task CreateShoppingCart(List<ShoppingCartItem> cart);
        public Task DeleteShoppingCart(int? id);
    }
}
