using AudioStore.DataAccess.Repository.IRepository;

namespace AudioStore.Services.Interfaces
{
    public interface IUnitOfWork
    {
        ICategoryRepository Category { get; }
        IProductRepository Product { get; }
        IManufacturerRepository Manufacturer { get; }
        IOrderDetailsRepository OrderDetails { get; }
        ICustomerRepository Customer { get; }

        Task SaveAsync();
    }
}
