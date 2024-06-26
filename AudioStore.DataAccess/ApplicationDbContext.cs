using AudioStore.Models;
using Microsoft.EntityFrameworkCore;

namespace AudioStore.DataAccess
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)        {
            
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Product> Products { get; set; }
      //  public DbSet<OrderDetails> OrderDetails { get; set; }
      //  public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}
