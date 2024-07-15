using AudioStore.Models;

namespace AudioStore.Services.Interfaces
{
    public interface IOrderDetailsService
    {
        public Task CreateOrderDetails(OrderDetails orderDetails);
        public Task DeleteOrderDetails(int? id);
        public Task<int> GetOrderDetailsID();
    }
}