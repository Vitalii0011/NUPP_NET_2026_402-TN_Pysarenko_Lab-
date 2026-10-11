using System.IO;
using Microsoft.EntityFrameworkCore;
using ClothingStore.Infrastructure.Models;

namespace ClothingStore.Infrastructure
{
    public class ClothingStoreContext : DbContext
    {
        public DbSet<ProductModel> Products => Set<ProductModel>();
        public DbSet<TShirtModel> TShirts => Set<TShirtModel>();
        public DbSet<ProductDetailsModel> ProductDetails => Set<ProductDetailsModel>();
        public DbSet<CategoryModel> Categories => Set<CategoryModel>();
        public DbSet<OrderModel> Orders => Set<OrderModel>();

        public string DbPath { get; }

        public ClothingStoreContext()
        {
            DbPath = Path.Combine(Directory.GetCurrentDirectory(), "clothing_store.db");
        }

        public ClothingStoreContext(DbContextOptions<ClothingStoreContext> options) : base(options)
        {
            DbPath = Path.Combine(Directory.GetCurrentDirectory(), "clothing_store.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite($"Data Source={DbPath}");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductModel>().UseTptMappingStrategy().ToTable("Products");
            modelBuilder.Entity<TShirtModel>().ToTable("TShirts");
            modelBuilder.Entity<ProductModel>()
                .HasOne(p => p.Details)
                .WithOne(d => d.Product)
                .HasForeignKey<ProductDetailsModel>(d => d.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<CategoryModel>()
                .HasMany(c => c.Products)
                .WithOne(p => p.Category)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<OrderModel>()
                .HasMany(o => o.Products)
                .WithMany(p => p.Orders)
                .UsingEntity(j => j.ToTable("OrderProducts"));
        }
    }
}