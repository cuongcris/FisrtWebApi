using Microsoft.EntityFrameworkCore;

namespace MyFirstWebApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }
        #region DBSet
        public DbSet<User> Users { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductType>  types { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(e =>
            {
                e.ToTable("Order");
                e.HasKey(o => o.OrderId);
                e.Property(o => o.CreatedDate).HasDefaultValue(DateTime.Now);
            });
            modelBuilder.Entity<OrderDetail>(e =>
            {
                e.ToTable("OrderDetail");
                e.HasKey(o => new { o.OrderId, o.ProductId });
                e.HasOne(r => r.Order)  //moi order detail co 1 order  1-m
                    .WithMany(r => r.OrderDetails)
                    .HasForeignKey(r => r.OrderId)
                    .HasConstraintName("FK_OrderDetai_Order");
                e.HasOne(r => r.Product) //moi order detail co 1 product 1-m
                    .WithMany(r => r.OrderDetails)
                    .HasForeignKey(r => r.ProductId)
                    .HasConstraintName("FK_OrderDetai_Product");
            });

            modelBuilder.Entity<User>(e =>
            {
                e.HasIndex(e => e.UserName).IsUnique();
            });
        }

    }
}
