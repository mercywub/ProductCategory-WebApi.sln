using Microsoft.EntityFrameworkCore;
using ProductCategory_WebApi.Domain.Models;
using ProductCategory_WebApi.Infrastructure.Datas;

namespace ProductCategory_WebApi.Infrastructure.Seeds
{
    public static class SeedData
    {
        public static async Task InitializeAsync(DBContext context)
        {
            context.Database.EnsureCreated();

            // =============================
            // 1️⃣ Product Categories
            // =============================
            if (!context.ProductCategories.Any())
            {
                var categories = new List<ProductCategory>
                {
                    new ProductCategory { Id = Guid.NewGuid(), CategoryName = "Electronics", CategoryDescription = "All kinds of electronic products" },
                    new ProductCategory { Id = Guid.NewGuid(), CategoryName = "Furniture", CategoryDescription = "Stylish and comfortable furniture" },
                    new ProductCategory { Id = Guid.NewGuid(), CategoryName = "Clothing", CategoryDescription = "Fashionable clothes for all ages" }
                };
                context.ProductCategories.AddRange(categories);
                await context.SaveChangesAsync();
            }

            // =============================
            // 2️⃣ Products
            // =============================
            if (!context.Products.Any())
            {
                var electronics = context.ProductCategories.First(c => c.CategoryName == "Electronics");
                var furniture = context.ProductCategories.First(c => c.CategoryName == "Furniture");
                var clothing = context.ProductCategories.First(c => c.CategoryName == "Clothing");

                var products = new List<Product>
                {
                    new Product { Id = Guid.NewGuid(), ProductName = "Smartphone", Description = "Latest smartphone", Price = 699, Discount = 10, Feature = true, Status = "Available", ProductImageFeatured = "smartphone.jpg", ProductCategoryId = electronics.Id },
                    new Product { Id = Guid.NewGuid(), ProductName = "Laptop", Description = "Gaming laptop", Price = 1200, Discount = 5, Feature = true, Status = "Available", ProductImageFeatured = "laptop.jpg", ProductCategoryId = electronics.Id },
                    new Product { Id = Guid.NewGuid(), ProductName = "Sofa Set", Description = "Comfortable sofa", Price = 899, Discount = 15, Feature = false, Status = "Available", ProductImageFeatured = "sofa.jpg", ProductCategoryId = furniture.Id },
                    new Product { Id = Guid.NewGuid(), ProductName = "Dining Table", Description = "Wooden dining table", Price = 499, Discount = 0, Feature = false, Status = "Available", ProductImageFeatured = "diningtable.jpg", ProductCategoryId = furniture.Id },
                    new Product { Id = Guid.NewGuid(), ProductName = "T-shirt", Description = "Cotton T-shirt", Price = 19.99m, Discount = 5, Feature = true, Status = "OutOfStock", ProductImageFeatured = "tshirt.jpg", ProductCategoryId = clothing.Id },
                    new Product { Id = Guid.NewGuid(), ProductName = "Jeans", Description = "Denim jeans", Price = 49.99m, Discount = 10, Feature = false, Status = "Available", ProductImageFeatured = "jeans.jpg", ProductCategoryId = clothing.Id }
                };
                context.Products.AddRange(products);
                await context.SaveChangesAsync();
            }

            // =============================
            // 3️⃣ Sizes
            // =============================
            if (!context.Sizes.Any())
            {
                var sizes = new List<Sizes>
                {
                    new Sizes { Id = Guid.NewGuid(), size = "Small" },
                    new Sizes { Id = Guid.NewGuid(), size = "Medium" },
                    new Sizes { Id = Guid.NewGuid(), size = "Large" },
                    new Sizes { Id = Guid.NewGuid(), size = "XL" }
                };
                context.Sizes.AddRange(sizes);
                await context.SaveChangesAsync();
            }

            // =============================
            // 4️⃣ Colors
            // =============================
            if (!context.Color.Any())
            {
                var colors = new List<Colors>
                {
                    new Colors { Id = Guid.NewGuid(), Text = "Red", Color = "#FF0000" },
                    new Colors { Id = Guid.NewGuid(), Text = "Blue", Color = "#0000FF" },
                    new Colors { Id = Guid.NewGuid(), Text = "Black", Color = "#000000" },
                    new Colors { Id = Guid.NewGuid(), Text = "White", Color = "#FFFFFF" }
                };
                context.Color.AddRange(colors);
                await context.SaveChangesAsync();
            }

            // =============================
            // 5️⃣ Galleries
            // =============================
            if (!context.Galleries.Any())
            {
                var galleries = new List<Gallary>
                {
                    new Gallary { Id = Guid.NewGuid(), Image = "product1.jpg", ImageUrl = "https://example.com/product1.jpg" },
                    new Gallary { Id = Guid.NewGuid(), Image = "product2.jpg", ImageUrl = "https://example.com/product2.jpg" },
                    new Gallary { Id = Guid.NewGuid(), Image = "product3.jpg", ImageUrl = "https://example.com/product3.jpg" },
                    new Gallary { Id = Guid.NewGuid(), Image = "product4.jpg", ImageUrl = "https://example.com/product4.jpg" }
                };
                context.Galleries.AddRange(galleries);
                await context.SaveChangesAsync();
            }

            // =============================
            // 6️⃣ Users & Carts
            // =============================
            if (!context.Users.Any())
            {
                var users = new List<User>
                {
                    new User { Id = Guid.NewGuid(), FullName = "Alice Johnson", Email = "alice@example.com", PasswordHash = "password123" },
                    new User { Id = Guid.NewGuid(), FullName = "Bob Smith", Email = "bob@example.com", PasswordHash = "password123" }
                };
                context.Users.AddRange(users);
                await context.SaveChangesAsync();

                // Create carts for users
                foreach (var user in users)
                {
                    context.Carts.Add(new Cart { Id = Guid.NewGuid(), UserId = user.Id });
                }
                await context.SaveChangesAsync();
            }

            // =============================
            // 7️⃣ Many-to-Many Product Relations
            // =============================
            var allProducts = context.Products.ToList();
            var allSizes = context.Sizes.ToList();
            var allColors = context.Color.ToList();
            var allGalleries = context.Galleries.ToList();

            foreach (var product in allProducts)
            {
                foreach (var size in allSizes)
                {
                    if (!context.ProductSizes.Any(ps => ps.ProductId == product.Id && ps.SizeId == size.Id))
                        context.ProductSizes.Add(new ProductSize { ProductId = product.Id, SizeId = size.Id });
                }

                foreach (var color in allColors)
                {
                    if (!context.ProductColors.Any(pc => pc.ProductId == product.Id && pc.ColorId == color.Id))
                        context.ProductColors.Add(new ProductColor { ProductId = product.Id, ColorId = color.Id });
                }

                foreach (var gallery in allGalleries)
                {
                    if (!context.ProductGalleries.Any(pg => pg.ProductId == product.Id && pg.GalleryId == gallery.Id))
                        context.ProductGalleries.Add(new ProductGallery { ProductId = product.Id, GalleryId = gallery.Id });
                }
            }
            await context.SaveChangesAsync();

            // =============================
            // 8️⃣ Addresses
            // =============================
            if (!context.Addresses.Any())
            {
                var userAlice = context.Users.First(u => u.Email == "alice@example.com");
                var addresses = new List<Address>
                {
                    new Address { Id = Guid.NewGuid(), UserId = userAlice.Id, Street = "123 Main St", City = "New York", Country = "USA" },
                    new Address { Id = Guid.NewGuid(), UserId = userAlice.Id, Street = "456 Park Ave", City = "New York", Country = "USA" }
                };
                context.Addresses.AddRange(addresses);
                await context.SaveChangesAsync();
            }

            // =============================
            // 9️⃣ CartItems
            // =============================
            if (!context.CartItems.Any())
            {
                var userAlice = context.Users.First(u => u.Email == "alice@example.com");
                var aliceCart = context.Carts.First(c => c.UserId == userAlice.Id);

                var cartItems = new List<CartItem>
                {
                    new CartItem { Id = Guid.NewGuid(), CartId = aliceCart.Id, ProductId = context.Products.First(p => p.ProductName == "Smartphone").Id, Quantity = 1 },
                    new CartItem { Id = Guid.NewGuid(), CartId = aliceCart.Id, ProductId = context.Products.First(p => p.ProductName == "T-shirt").Id, Quantity = 2 }
                };
                context.CartItems.AddRange(cartItems);
                await context.SaveChangesAsync();
            }

            // =============================
            // 🔟 Orders & OrderItems
            // =============================
            if (!context.Orders.Any())
            {
                var userAlice = context.Users.First(u => u.Email == "alice@example.com");
                var address = context.Addresses.First(a => a.UserId == userAlice.Id);

                var order = new Order
                {
                    Id = Guid.NewGuid(),
                    UserId = userAlice.Id,
                    AddressId = address.Id,
                    TotalAmount = 1500,
                    Status = "Pending",
                    OrderDate = DateTime.UtcNow
                };
                context.Orders.Add(order);
                await context.SaveChangesAsync();

                var orderItems = allProducts.Take(3).Select(p => new OrderItem
                {
                    Id = Guid.NewGuid(),
                    OrderId = order.Id,
                    ProductId = p.Id,
                    Quantity = 1,
                    Price = p.Price
                }).ToList();

                context.OrderItems.AddRange(orderItems);
                await context.SaveChangesAsync();

                // =============================
                // 1️⃣1️⃣ Payment
                // =============================
                var payment = new Payment
                {
                    Id = Guid.NewGuid(),
                    OrderId = order.Id,
                    Amount = order.TotalAmount,
                    TransactionId = Guid.NewGuid().ToString(),
                    PaymentMethod = "Credit Card",
                    PaymentDate = DateTime.UtcNow,
                    Status = "Completed"
                };
                context.Payments.Add(payment);
                await context.SaveChangesAsync();
            }
        }
    }
}
