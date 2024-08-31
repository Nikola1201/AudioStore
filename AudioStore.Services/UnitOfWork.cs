using AudioStore.DataAccess;
using AudioStore.Models;
using AudioStore.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioStore.Services
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Category = new CategoryServices(_db);
            Manufacturer = new ManufacturerServices(_db);
            Product = new ProductServices(_db);
            ApplicationUser = new ApplicationUserService(_db);
            OrderDetails = new OrderDetailsService(_db);

        }
        public ICategoryService Category { get; private set; }
        public IManufacturerService Manufacturer { get;private set; }
        public IProductService Product { get; private set; }
        public IApplicationUserService ApplicationUser { get;private set; }
        public IOrderDetailsService OrderDetails { get;private set; }

        public void Save()
        {
            _db.SaveChangesAsync();
        }
    }
}
