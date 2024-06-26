using AudioStore.DataAccess;
using AudioStore.Models;
using AudioStore.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AudioStore.Services
{
    public class ManufacturerServices : IManufacturerService
    {
        public ApplicationDbContext Context { get; }
        public ManufacturerServices(ApplicationDbContext _context)
        {
            Context = _context;
        }
        public async Task<Manufacturer> CreateManufacturer(Manufacturer manufacturer)
        {
            Context.Manufacturers.Add(manufacturer);
            await Context.SaveChangesAsync();
            return manufacturer;
        }

        public async Task DeleteManufacturer(int? id)
        {
            var obj= await Context.Manufacturers.FindAsync(id);
            if (obj != null)
            {
                Context.Manufacturers.Remove(obj);
                await Context.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException($"Manufacturer with ID {id} doesn't exist!");
            }
        }

        public Task<List<Manufacturer>> GetAllManufacturers()
        {
            return Context.Manufacturers.ToListAsync();
        }

        public Task<Manufacturer> GetManufacturerById(int? id)
        {
            return Context.Manufacturers.FirstOrDefaultAsync(x => x.ManufacturerID == id);
        }

        public async Task<Manufacturer> UpdateManufacturer(Manufacturer manufacturer)
        {
            Context.Update(manufacturer);
            await Context.SaveChangesAsync();
            return manufacturer;
        }
    }
}
