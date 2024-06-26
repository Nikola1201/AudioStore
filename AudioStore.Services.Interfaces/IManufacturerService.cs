using AudioStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioStore.Services.Interfaces
{
    public interface IManufacturerService
    {
        public Task<Manufacturer> CreateManufacturer(Manufacturer manufacturer);
        public Task<List<Manufacturer>> GetAllManufacturers();
        public Task<Manufacturer> GetManufacturerById(int? id);
        public Task<Manufacturer> UpdateManufacturer(Manufacturer manufacturer);
        public Task DeleteManufacturer(int? id);

    }
}
