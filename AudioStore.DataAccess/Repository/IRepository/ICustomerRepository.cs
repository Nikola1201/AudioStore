using AudioStore.DataAccess.Repository.IRepository;
using AudioStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioStore.DataAccess.Repository.IRepository
{
    public interface ICustomerRepository:IRepository<Customer>
    {
        public int GetApplicationUserID(Customer user);
    }
}
