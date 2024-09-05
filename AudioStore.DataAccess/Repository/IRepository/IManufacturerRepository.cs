using AudioStore.Models;

namespace AudioStore.DataAccess.Repository.IRepository
{
    public interface IManufacturerRepository : IRepository<Manufacturer>
    {
        public Task<Manufacturer> UpdateManufacturer(Manufacturer manufacturer);
    }
}
