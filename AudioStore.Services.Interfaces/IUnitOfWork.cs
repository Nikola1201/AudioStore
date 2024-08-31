using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioStore.Services.Interfaces
{
    public interface IUnitOfWork
    {
        ICategoryService Category { get; }
        IProductService Product { get; }
        IManufacturerService Manufacturer { get; }
        IOrderDetailsService OrderDetails { get; }
        IApplicationUserService ApplicationUser { get; }

        void Save();
    }
}
