using AudioStore.Models;

namespace AudioStore.Services.Interfaces
{
    public interface IShoppingCartService
    {
        public Task CreateShoppingCart(List<ShoppingCart> cart);
        public Task DeleteShoppingCart(int? id);
    }
}
