using AudioStore.DataAccess.Repository.IRepository;
using AudioStore.Models;

namespace AudioStore.DataAccess.Repository
{
    public class OrderDetailsRepository : Repository<OrderDetails>, IOrderDetailsRepository
    {
        private readonly ApplicationDbContext _db;

        public OrderDetailsRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }
        public override async Task AddAsync(OrderDetails entity)
        {
            
            await _db.OrderDetails.AddAsync(entity);
        }

  
        public void Update(OrderDetails orderDetails)
        {
            _db.OrderDetails.Update(orderDetails);
        }
    }
}
