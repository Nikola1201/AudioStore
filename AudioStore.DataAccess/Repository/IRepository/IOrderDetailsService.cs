using AudioStore.DataAccess.Repository.IRepository;
using AudioStore.Models;

namespace AudioStore.DataAccess.Repository.IRepository
{
    public interface IOrderDetailsRepository:IRepository<OrderDetails>
    {
        public void Update(OrderDetails orderDetails);
    }
}