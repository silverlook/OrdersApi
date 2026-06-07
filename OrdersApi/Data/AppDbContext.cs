using Microsoft.EntityFrameworkCore;
using OrdersApi.Entities;

namespace OrdersApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Client> Clients { get; set; } 
        public DbSet<Order> Orders { get; set; }
        public DbSet<Product> Products { get; set; } 
        public DbSet<Status> Statuses { get; set; } 
        public DbSet<ProductOrder> ProductOrders { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>(e =>
            {
                e.ToTable("Client");
                e.Property(c => c.FirstName).HasMaxLength(50).IsRequired();
                e.Property(c => c.LastName).HasMaxLength(100).IsRequired();
            });
            
            modelBuilder.Entity<Status>(e =>
            {
                e.ToTable("Status");
                e.Property(s => s.Name).HasColumnName("Nam").HasMaxLength(50).IsRequired();
            });

            modelBuilder.Entity<Product>(e =>
            {
                e.ToTable("Product");
                e.Property(p => p.Name).HasMaxLength(50).IsRequired();
                e.Property(p => p.Price).HasColumnType("numeric(10,2)");
            });

            modelBuilder.Entity<Order>(e =>
            {
                e.ToTable("Order");
                e.Property(o => o.CreatedAt).HasColumnType("datetime");
                e.Property(o => o.FulfilledAt).HasColumnType("datetime").IsRequired(false);

                e.HasOne(o => o.Client)
                 .WithMany(c => c.Orders)
                 .HasForeignKey(o => o.ClientId)
                 .OnDelete(DeleteBehavior.Restrict);

                e.HasOne(o => o.Status)
                 .WithMany(s => s.Orders)
                 .HasForeignKey(o => o.StatusId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ProductOrder>(e =>
            {
                e.ToTable("ProductOrder");

                e.HasKey(po => new { po.ProductId, po.OrderId });

                e.HasOne(po => po.Product)
                 .WithMany(p => p.ProductOrders)
                 .HasForeignKey(p => p.ProductId);

                e.HasOne(po => po.Order)
                 .WithMany(p => p.ProductOrders)
                 .HasForeignKey(p => p.OrderId);

                e.Property(po => po.Amount)
                 .IsRequired();
            });
        }

    }
}
