using AudioStore.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace AudioStore.DataAccess
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)        {
            
        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShoppingCart>()
            .HasOne(s => s.OrderDetails)
            .WithMany(o => o.Carts)
            .HasForeignKey(s => s.OrderDetailsID);

            modelBuilder.Entity<ShoppingCart>()
                .HasOne(s => s.Product)
                .WithMany(p => p.ShoppingCartItems)
                .HasForeignKey(s => s.ProductID);

            base.OnModelCreating(modelBuilder);
        }
    }
}
