using AudioStore.DataAccess.Repository.IRepository;
using AudioStore.Models;

namespace AudioStore.DataAccess.Repository
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        private ApplicationDbContext _db;
        public CustomerRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public int GetApplicationUserID(Customer user)
        {
            var x = _db.Customers.FirstOrDefault(u => u.Email == user.Email);
            return x.CustomerID;
        }
    }
}
