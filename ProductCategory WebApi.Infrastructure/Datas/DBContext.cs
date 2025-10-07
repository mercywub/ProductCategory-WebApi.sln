using Microsoft.EntityFrameworkCore;
using ProductCategory_WebApi.Domain.Models;

namespace ProductCategory_WebApi.Infrastructure.Datas
{
    public class DBContext : DbContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }
        public DbSet<Colors> Color { get; set; }
        public DbSet<Sizes> Sizes { get; set; }
        public DbSet<Gallary> Galleries { get; set; }

        public DbSet<ProductColor> ProductColors { get; set; }
        public DbSet<ProductSize> ProductSizes { get; set; }
        public DbSet<ProductGallery> ProductGalleries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Product ↔ Category (One-to-Many)
            modelBuilder.Entity<Product>()
                .HasOne(p => p.ProductCategory)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.ProductCategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // Product ↔ Color (Many-to-Many)
            modelBuilder.Entity<ProductColor>()
                .HasKey(pc => new { pc.ProductId, pc.ColorId });

            modelBuilder.Entity<ProductColor>()
                .HasOne(pc => pc.Product)
                .WithMany(p => p.ProductColors)
                .HasForeignKey(pc => pc.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductColor>()
                .HasOne(pc => pc.Colors)
                .WithMany(c => c.ProductColors)
                .HasForeignKey(pc => pc.ColorId)
                .OnDelete(DeleteBehavior.Cascade);

            // Product ↔ Size (Many-to-Many)
            modelBuilder.Entity<ProductSize>()
                .HasKey(ps => new { ps.ProductId, ps.SizeId });

            modelBuilder.Entity<ProductSize>()
                .HasOne(ps => ps.Product)
                .WithMany(p => p.ProductSizes)
                .HasForeignKey(ps => ps.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductSize>()
                .HasOne(ps => ps.Size)
                .WithMany(s => s.ProductSizes)
                .HasForeignKey(ps => ps.SizeId)
                .OnDelete(DeleteBehavior.Cascade);

            // Product ↔ Gallery (Many-to-Many)
            modelBuilder.Entity<ProductGallery>()
                .HasKey(pg => new { pg.ProductId, pg.GalleryId });

            modelBuilder.Entity<ProductGallery>()
                .HasOne(pg => pg.Product)
                .WithMany(p => p.ProductGalleries)
                .HasForeignKey(pg => pg.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductGallery>()
                .HasOne(pg => pg.Gallary)
                .WithMany(g => g.ProductGalleries)
                .HasForeignKey(pg => pg.GalleryId)
                .OnDelete(DeleteBehavior.Cascade);

            // Decimal config
            modelBuilder.Entity<Product>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Product>()
                .Property(p => p.Discount)
                .HasColumnType("decimal(18,2)");
        }
    }
}
