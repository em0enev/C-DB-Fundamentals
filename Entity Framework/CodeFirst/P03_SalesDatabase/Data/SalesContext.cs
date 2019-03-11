using Microsoft.EntityFrameworkCore;
using P03_SalesDatabase.Data.Models;

namespace P03_SalesDatabase.Data
{
    public class SalesContext : DbContext
    {
        public DbSet<Product> Products { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Store> Stores { get; set; }

        public DbSet<Sale> Sales { get; set; }

        public SalesContext()
        {

        }

        public SalesContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-VNP1D7N\SQLEXPRESS;Database=Sales;Integrated Security = true;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            CreateCustomerEntity(modelBuilder);

            CreateProductEntity(modelBuilder);

            CreateStoreEntity(modelBuilder);

            CreateSaleEntity(modelBuilder);
        }

        private void CreateCustomerEntity(ModelBuilder modelBuilder)
        {

            modelBuilder.
                Entity<Customer>()
                .HasKey(c => c.CustomerId);

            modelBuilder.
                Entity<Customer>()
                .Property(c => c.Name)
                .HasMaxLength(100)
                .IsUnicode();

            modelBuilder.
                Entity<Customer>()
                .Property(c => c.Email)
                .HasMaxLength(80);
        }

        private void CreateProductEntity(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Product>()
                .HasKey(p => p.ProductId);

            modelBuilder.
                Entity<Product>()
                .Property(c => c.Name)
                .HasMaxLength(50)
                .IsUnicode();

            modelBuilder
                .Entity<Product>()
                .Property(p => p.Discription)
                .HasDefaultValue("No description")
                .HasMaxLength(250);
        }

        private void CreateStoreEntity(ModelBuilder modelBuilder)
        {
            modelBuilder
              .Entity<Store>()
              .HasKey(p => p.StoreId);

            modelBuilder.
                Entity<Store>()
                .Property(s => s.Name)
                .HasMaxLength(80)
                .IsUnicode();
        }

        private void CreateSaleEntity(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Sale>()
                .HasKey(s => s.SaleId);

            modelBuilder
                .Entity<Sale>()
                .Property(s => s.Date)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder
                .Entity<Sale>()
                .HasOne(s => s.Product)
                .WithMany(s => s.Sales)
                .HasForeignKey(s => s.ProductId);

            modelBuilder
                .Entity<Sale>()
                .HasOne(s => s.Customer)
                .WithMany(s => s.Sales)
                .HasForeignKey(s => s.CustomerId);

            modelBuilder
                .Entity<Sale>()
                .HasOne(s => s.Store)
                .WithMany(s => s.Sales)
                .HasForeignKey(s => s.StoreId);
        }
    }
}
