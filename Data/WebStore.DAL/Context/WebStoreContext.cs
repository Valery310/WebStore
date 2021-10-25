using Microsoft.EntityFrameworkCore;
using WebStore.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using WebStore.Domain.Entities;

namespace WebStore.DAL.Context
{
    public class WebStoreContext : IdentityDbContext<User, Role, string>
    {
        public WebStoreContext(DbContextOptions<WebStoreContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }

        public DbSet<Section> Sections { get; set; }
        
        public DbSet<Brand> Brands { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder model)
        {
            base.OnModelCreating(model);

            model.Entity<Order>()
               .HasMany(order => order.OrderItems)
               .WithOne(item => item.Order)
               .OnDelete(DeleteBehavior.Cascade);

            model.Entity<User>()
               .HasMany<Order>()
               .WithOne(order => order.User)
               .OnDelete(DeleteBehavior.Cascade);

            model.Entity<OrderItem>()
               .HasOne(item => item.Product)
               .WithMany()
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
