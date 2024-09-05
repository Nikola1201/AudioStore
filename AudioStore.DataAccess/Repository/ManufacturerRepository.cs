using AudioStore.DataAccess;
using AudioStore.DataAccess.Repository;
using AudioStore.Models;
using AudioStore.DataAccess.Repository.IRepository;

    public class ManufacturerRepository :Repository<Manufacturer>, IManufacturerRepository
    {
        private readonly ApplicationDbContext _db;

        public ManufacturerRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        } 
        public async Task<Manufacturer> UpdateManufacturer(Manufacturer manufacturer)
        {
            _db.Manufacturers.Update(manufacturer);
            await _db.SaveChangesAsync();
            return manufacturer;
        }
    }

